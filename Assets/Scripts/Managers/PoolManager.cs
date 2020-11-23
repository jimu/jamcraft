using System.Collections.Generic;
using UnityEngine;

/**
 * Maintains a list of Pools keyed on prefabs
 */

public class PoolManager : MonoSingleton<PoolManager>
{
    public Dictionary<GameObject, Pool> Pools = new Dictionary<GameObject, Pool>();

    public Pool GetPool(GameObject prefab)
    {
        return Pools.ContainsKey(prefab) ? 
            Pools[prefab] :
            Pools[prefab] = new Pool(prefab, transform);
    }

    public GameObject Get(GameObject prefab, Vector3 position)
    {
        return GetPool(prefab).Get(position);
    }
    public GameObject Get(GameObject prefab, Transform parent)
    {
        return GetPool(prefab).Get(parent);
    }
}
