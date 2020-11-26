using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public abstract class CraftableData : ScriptableObject
{
    [Header("Crafting")]
    [Tooltip("Used if icon not specifed or 'always' set")]
    public string description;
    [Tooltip("Always label crafting output")]
    public bool alwaysUseDescription;
    public float craftingPriority;
    //public List<ItemData> ingredients = new List<ItemData>();
    public List<CraftableData> ingredients = new List<CraftableData>();
    [Tooltip("Optional 32x32 inventory icon")]
    public Sprite icon32;
    [Tooltip("Optional 64x64 inventory icon")]
    public Sprite icon64;
    [Tooltip("Output icon (128x128)")]
    public Sprite icon128;



    public bool IsSupplied(Dictionary<CraftableData, int> supplies)
    {
        Dictionary<CraftableData, int> workingSupplies = new Dictionary<CraftableData, int>(supplies);

        foreach (CraftableData ingredient in ingredients)
            if (!workingSupplies.ContainsKey(ingredient) || --workingSupplies[ingredient] < 0)
                return false;

        // Debug.Log($"We can make a {name}");
        return true;
    }




    public Sprite GetSmallestIcon(Sprite defaultIcon = null)
    {
        return icon32 != null ? icon32 : icon64 != null ? icon64 : icon128 != null ? icon128 : defaultIcon;
    }

    public Sprite GetLargestIcon(Sprite defaultIcon = null)
    {
        return icon128 != null ? icon128 : icon64 != null ? icon64 : icon32 != null ? icon32 : defaultIcon;
    }
}

