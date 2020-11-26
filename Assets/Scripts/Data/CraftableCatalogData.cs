using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName = "New Catalog", menuName = "Jamcraft/CraftableCatalog")]


// This is a list of craftable item
// It is used to restrict what Craftable items that Crafter can craft for a particular level
public class CraftableCatalogData : ScriptableObject
{
    public List<CraftableData> catalog;

    public List<CraftableData> GetSortedCatalog()
    {
        return catalog.OrderBy(x => -x.craftingPriority).ToList();
    }

}
