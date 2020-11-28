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

    
    // sorted catalog
    List<CraftableData> catalog;

    CraftableData craftOutput;
    Dictionary<CraftableData, int> workbenchItems = new Dictionary<CraftableData, int>();
        


    public CraftableData RefreshOutput()
    {
        InventoryWorkbench();

        //Debug.Log($"RefreshRecipe: There are {catalog.Count} recipes. My name is {name}");

        foreach (CraftableData o in catalog)
        {
            //            Debug.Log($"Checking recipe {o.name}: {o.IsSupplied(workbenchItems)}");
            if (o.IsSupplied(workbenchItems))
                return SetOutput(o);
        }

        Debug.Log($"Returning {(CanConstructBot() ? botOutputPlaceholderData.name : "null")}");
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

    public void Craft()
    {
        Debug.Log($"I'm going to craft a {craftOutput.name} ({craftOutput.description})");

        ConsumeResources(craftOutput);

        if (craftOutput == botOutputPlaceholderData)
            ConstructBot();
        else
            AddToInventory(craftOutput);

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
        Debug.Log($"CanConstructBot");
        foreach (var item in workbenchContainer.GetComponentsInChildren<Item>())
        {
            Debug.Log($"* {item}");
            if (item.Data is ChassisData)
            {
                Debug.Log($" * Returning true because {item.Data.name} is ChassisData");
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
        /*
        if (chassisData.weaponHardpoint != "" && weapon1 != null)
        {
            WeaponData weaponData = weapon1.Data as WeaponData;
            Transform pivot = botObject.transform.Find(chassisData.weaponHardpoint);
            GameObject o = PoolManager.Instance.Get(weaponData.prefab, pivot);
            botData.muzzle1 = o.GetComponent<Weapon>().muzzle;
            //if (weaponData.fireRate > 0)
                //pivot.gameObject.AddComponent<DebugTurretRotator>();
            DestroyImmediate(weapon1.gameObject);
            botData.weapon1 = weaponData;
        }
        if (chassisData.weaponHardpoint2 != "" && weapon2 != null)
        {
            WeaponData weaponData = weapon2.Data as WeaponData;
            Transform pivot = botObject.transform.Find(chassisData.weaponHardpoint2);
            GameObject o = PoolManager.Instance.Get(weaponData.prefab, pivot);
            botData.muzzle2 = o.transform.Find("firePointA");

            //if (weaponData.fireRate > 0)
            //pivot.gameObject.AddComponent<DebugTurretRotator>();
            DestroyImmediate(weapon2.gameObject);
            botData.weapon2 = weaponData;
        }
        if (chassisData.armourHardpoint != "" && armour != null)
        {
            ArmourData armourData = armour.Data as ArmourData;
            Transform pivot = botObject.transform.Find(chassisData.armourHardpoint);
            PoolManager.Instance.Get(armourData.prefab, pivot);
            DestroyImmediate(armour.gameObject);
            botData.armour = armourData;
        }

        if (botData.weapon1 || botData.weapon2)
            botObject.AddComponent<Targeter>();
        */
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
    }

}
