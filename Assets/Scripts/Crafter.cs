using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


#pragma warning disable 649

public class Crafter : MonoSingleton<Crafter>
{
    [SerializeField] Transform workbenchContainer;
    [SerializeField] Transform inventoryContainer;
    [SerializeField] Image outputSprite;
    [SerializeField] Sprite noOutputSprite;
    [SerializeField] Sprite TBDSprite;
    [SerializeField] TextMeshProUGUI outputDescriptionText;
    [SerializeField] Button craftButton;
    [SerializeField] Item inventoryItemPrefab;

    [Tooltip("List of bot components that this crafter can make")]
    [SerializeField] CraftableCatalogData catalogData;
    [SerializeField] CraftableData botPlaceholderData;

    public Action<GameObject> onNewBot;

    // sorted catalog
    List<CraftableData> catalog;

    CraftableData craftOutput;
    Dictionary<CraftableData, int> workbenchItems = new Dictionary<CraftableData, int>();


    // subscribe to onNewBot to be notified when new bots are released into the wild


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


    public CraftableData RefreshOutput()
    {

        InventoryWorkbench();

        //Debug.Log($"RefreshRecipe: There are {catalog.Count} recipes. My name is {name}");

        foreach (CraftableData o in catalog) {
//            Debug.Log($"Checking recipe {o.name}: {o.IsSupplied(workbenchItems)}");
            if (o.IsSupplied(workbenchItems))
                return SetOutput(o);
        }

        return SetOutput(CanConstructBot() ? botPlaceholderData : null);
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

        if (craftOutput == botPlaceholderData)
        {
            ConstructBot();
            return;
        }

        Transform slot = FindAnEmptyInventorySlot();
        Debug.Assert(slot != null, "Crafting without available inventory slot. Craft button should have been disabled?");

        if (slot != null)
        {
            ConsumeResources(craftOutput);
            Item item = Instantiate(inventoryItemPrefab, slot);
            item.Data = craftOutput;
            item.GetComponent<Image>().sprite = craftOutput.GetSmallestIcon(TBDSprite);
            RefreshOutput();
        }
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
                return true;
        }
        return false;
    }

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

    void ConstructBotFromParts(Item chassis, Item weapon1, Item weapon2, Item armour)
    {
        ChassisData chassisData = chassis.Data as ChassisData;

        GameObject bot = PoolManager.Instance.Get(chassisData.prefab, Vector3.zero);
        Bot botData = bot.AddComponent<Bot>();
        botData.chassis = chassisData;

        if (chassisData.weaponHardpoint != "" && weapon1 != null)
        {
            WeaponData weaponData = weapon1.Data as WeaponData;
            Transform pivot = bot.transform.Find(chassisData.weaponHardpoint);
            PoolManager.Instance.Get(weaponData.prefab, pivot);
            if (weaponData.fireRate > 0)
                pivot.gameObject.AddComponent<DebugTurretRotator>();
            DestroyImmediate(weapon1.gameObject);
            botData.weapon1 = weaponData;
        }
        if (chassisData.weaponHardpoint2 != "" && weapon2 != null)
        {
            WeaponData weaponData = weapon2.Data as WeaponData;
            Transform pivot = bot.transform.Find(chassisData.weaponHardpoint2);
            PoolManager.Instance.Get(weaponData.prefab, pivot);
            if (weaponData.fireRate > 0)
                pivot.gameObject.AddComponent<DebugTurretRotator>();
            DestroyImmediate(weapon2.gameObject);
            botData.weapon2 = weaponData;
        }
        if (chassisData.armourHardpoint != "" && armour != null)
        {
            ArmourData armourData = armour.Data as ArmourData;
            Transform pivot = bot.transform.Find(chassisData.armourHardpoint);
            PoolManager.Instance.Get(armourData.prefab, pivot);
            DestroyImmediate(armour.gameObject);
            botData.armour = armourData;
        }

        DestroyImmediate(chassis.gameObject);
        RefreshOutput();
        onNewBot?.Invoke(bot);
    }
}
