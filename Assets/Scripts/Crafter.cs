using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


#pragma warning disable 649

/**
 * INTERFACE
 *   Action<Bot> onNewBot - subscribe to this to be notified when a Bot has been created
 *   
 *  Add items to inventory:
 *  
 *   Crafter.Instance.AddToInventory(CraftableObject)
 *   Crafter.Instance.AddToInventory(CraftableObject[])
 *   Crafter.Instance.AddToInventory(List<CraftableObject>)
 *  
 *  Use DebugInventoryRewards.cs to add cheat buttons that add resources to inventory
 *   
 * 
 * KNOWS ABOUT
 * CraftableCatalogData - List of things that can be built
 * CraftableData botOutputPlaceholderData - This is a generic sprite used for bots without an image
 */

public class Crafter : MonoSingleton<Crafter>
{
    // subscribe to onNewBot to be notified when new bots are released into the wild
    public Action<Bot> onNewBot;
    
    [SerializeField] Transform workbenchContainer;  // GameObject Containing Inventory Items
    [SerializeField] Transform inventoryContainer;
    [SerializeField] Image outputSprite;
    [SerializeField] Sprite noOutputSprite;
    [SerializeField] Sprite TBDSprite;
    [SerializeField] TextMeshProUGUI outputDescriptionText;
    [SerializeField] Button craftButton;
    [SerializeField] Item inventoryItemPrefab;

    [Tooltip("List of bot components that this crafter can make")]
    [SerializeField] CraftableCatalogData catalogData;
    [SerializeField] CraftableData botOutputPlaceholderData;
    [SerializeField] FlyoutOutputPanel flyoutOutputPanel;

    // Extra Output Button
    [Header("Extra Output Flyout Button")]
    [SerializeField] GameObject extraOutputButton;
    [SerializeField] TextMeshProUGUI extraOutputText;

    List<CraftableData> suppliedOutputList = new List<CraftableData>();

    // sorted catalog
    List<CraftableData> catalog;

    CraftableData craftOutput;
    Dictionary<CraftableData, int> workbenchItems = new Dictionary<CraftableData, int>();

    public CraftableData RefreshOutput()
    {
        InventoryWorkbench();
        suppliedOutputList.Clear();

        //Debug.Log($"RefreshRecipe: There are {catalog.Count} recipes. My name is {name}");

        foreach (CraftableData output in catalog)
        {
            //            Debug.Log($"Checking recipe {o.name}: {o.IsSupplied(workbenchItems)}");
            if (output.IsSupplied(workbenchItems))
            {
                suppliedOutputList.Add(output);
            }
        }

        SetExtraOutput(suppliedOutputList.Count);
        UpdateOutputFlyout();

        if (suppliedOutputList.Count > 0)
        {
            return SetOutput(suppliedOutputList[0]);
        }

//        Debug.Log($"Returning {(CanConstructBot() ? botOutputPlaceholderData.name : "null")}");
        return SetOutput(CanConstructBot() ? botOutputPlaceholderData : null);

    }

    protected override void Init()
    {
        catalog = catalogData.GetSortedCatalog();
        craftButton.interactable = false;
        Item.SetDefaultSprite(TBDSprite);
    }



    // stores item counts in dictionary workbenchItems
    void InventoryWorkbench()
    {
        workbenchItems.Clear();
        foreach(var item in workbenchContainer.GetComponentsInChildren<Item>())
        {
            int count;
            workbenchItems.TryGetValue(item.Data, out count);
            workbenchItems[item.Data] = count + 1;
        }
    }


    CraftableData SetOutput(CraftableData output)
    {
        craftOutput = output;
//        Debug.Log($"  Output set to {output?.name}");

        if (output == null)
        {
            outputDescriptionText.text = "";
            craftButton.interactable = false;
            outputSprite.sprite = noOutputSprite;
        }
        else
        {
            outputSprite.sprite = output.GetLargestIcon(TBDSprite);
            outputDescriptionText.text = output.alwaysUseDescription || outputSprite.sprite == TBDSprite ? output.description : "";
            craftButton.interactable = true;
        }
        return output;
    }

    public void Craft(CraftableData data = null)
    {
        if (data)
            SetOutput(data);
        else
            data = craftOutput;

//        Debug.Log($"I'm going to craft a {data.name} ({data.description})");

        ConsumeResources(data);

        if (craftOutput == botOutputPlaceholderData)
            ConstructBot();
        else
            AddToInventory(data);

        CloseFlyout();
        RefreshOutput();

    }

    // destroys workbench items in recipe
    // assumes ingredients are available. unavailable ingrediates are ignored.
    void ConsumeResources(CraftableData data)
    {
        foreach (CraftableData ingredient in data.ingredients)
        {
            foreach (var item in workbenchContainer.GetComponentsInChildren<Item>())
            {
                if (item.Data == ingredient)
                {
                    DestroyImmediate(item.gameObject);
                    break;
                }
            }
        }
    }

    Transform FindAnEmptyInventorySlot()
    {
        for(int i = 0; i < inventoryContainer.childCount; ++i)
        {
            Transform child = inventoryContainer.GetChild(i);
            if (child.childCount == 0)
                return child;
        }
        return null;
    }


    bool CanConstructBot()
    {
        foreach (var item in workbenchContainer.GetComponentsInChildren<Item>())
        {
            if (item.Data is ChassisData)
            {
                //Debug.Log($" * Returning true because {item.Data.name} is ChassisData");
                return true;
            }
        }
        return false;
    }

    // Checks workbench for a valid bot combination
    void ConstructBot()
    {
        Item weapon1, weapon2, chassis, armour;
        weapon1 = weapon2 = chassis = armour = null;

        foreach (var item in workbenchContainer.GetComponentsInChildren<Item>())
        {
            if (item.Data is WeaponData)
            {
                if (weapon1 == null)
                    weapon1 = item;
                else
                    weapon2 = item;
            }
            else if (item.Data is ChassisData)
                chassis = item;
            else if (item.Data is ArmourData)
                armour = item;
        }

        if (chassis)
            ConstructBotFromParts(chassis, weapon1, weapon2, armour);
    }


    WeaponData AttachWeapon(GameObject botObject, string hardpointName, Item item, Bot botData)
    {
        if (hardpointName != "" && item != null)
        {
            WeaponData weaponData = item.Data as WeaponData;
            Transform pivot = botData.transform.Find(hardpointName);
            GameObject o = PoolManager.Instance.Get(weaponData.prefab, pivot);
            Debug.Assert(o.GetComponent<Weapon>() != null, "Weapon Prefab requires Weapon Component");
            botData.muzzle1 = o.GetComponent<Weapon>().muzzle;
            DestroyImmediate(item.gameObject);
            return weaponData;
        }
        return null;

    }

    ArmourData AttachArmour(GameObject botObject, string hardpointName, Item item, Bot botData)
    { 
        if (hardpointName != "" && item != null)
        {
            ArmourData armourData = item.Data as ArmourData;
            Transform pivot = botObject.transform.Find(hardpointName);
            PoolManager.Instance.Get(armourData.prefab, pivot);
            DestroyImmediate(item.gameObject);
            return armourData;
        }
        return null;
    }

void ConstructBotFromParts(Item chassis, Item weapon1, Item weapon2, Item armour)
    {
        ChassisData chassisData = chassis.Data as ChassisData;

        GameObject botObject = PoolManager.Instance.Get(chassisData.prefab, Vector3.zero);
        Bot botData = botObject.AddComponent<Bot>();
        botData.chassis = chassisData;
        botData.owner = GameObject.Find("Player").GetComponent<PlayerBase>(); // TODO HACK

        botData.weapon1 = AttachWeapon(botObject, "WeaponHardpoint1", weapon1, botData);
        botData.weapon2 = AttachWeapon(botObject, "WeaponHardpoint2", weapon2, botData);
        botData.armour  = AttachArmour(botObject, "ArmourHardpoint",  armour,  botData);
        DestroyImmediate(chassis.gameObject);
        RefreshOutput();
        onNewBot?.Invoke(botData);
    }

    // Adds a single CraftableData to the inventory if there is room
    public void AddToInventory(CraftableData itemData)
    {
        Transform slot = FindAnEmptyInventorySlot();
        Debug.Assert(slot != null, $"Discarding item {itemData.name}");

        if (slot != null)
        {
            Item item = Instantiate(inventoryItemPrefab, slot);

            item.Data = itemData;
            item.GetComponent<Image>().sprite = itemData.GetSmallestIcon(TBDSprite);
        }

    }

    // Adds a list of CraftableData to the inventory. Excess items are discarded
    public void AddToInventory(IEnumerable<CraftableData> itemsData)
    {
        foreach(CraftableData item in itemsData)
            AddToInventory(item);
        UpdateOutputFlyout();
    }

    public void OutputClicked(CraftableData data)
    {
        CloseFlyout();
        Craft(data);
        Debug.Log($"Crafter output: {data.name}");
    }

    void CloseFlyout()
    {
        flyoutOutputPanel.Deactivate();
    }

    public void ActivateOutputFlyout()
    {
        if (suppliedOutputList.Count > 1)
            flyoutOutputPanel.Activate(suppliedOutputList);
        else
            flyoutOutputPanel.Deactivate();
    }

    void UpdateOutputFlyout()
    {
        if (flyoutOutputPanel.isActiveAndEnabled)
            ActivateOutputFlyout();
    }

    void SetExtraOutput(int count)
    {
        extraOutputButton.SetActive(count > 1);
        if (count > 1)
            extraOutputText.text = "+" + (count - 1).ToString();
    }
}
