using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#pragma warning disable 0649

// implements a pool for a specific prefab
// used by PoolManager
public class Pool
{
    GameObject prefab;
    Transform parent;

    public Pool(GameObject prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
    }

    // Note: msdn documentation claims Lists use an array internally and should perform just as fast
    List<GameObject> pool = new List<GameObject>();
    int lastIndex = -1;

    public GameObject Get(Vector3 position)
    {
        GameObject o;
        //Debug.Log($"pool: {pool}");
        for (int count = pool.Count; count-- > 0;)
        {
            lastIndex = (lastIndex + 1) % pool.Count;
            if (pool[lastIndex] == null)
                return pool[lastIndex] = GameObject.Instantiate(prefab, position, Quaternion.identity, parent);
            if (!pool[lastIndex].activeSelf)
            {
                o = pool[lastIndex];
                o.transform.position = position;
                o.SetActive(true);
                return o;
            }
        }
        o = GameObject.Instantiate(prefab, position, Quaternion.identity, parent);
        pool.Add(o);
        return o;
    }

    public GameObject Get(Transform newTransform)
    {
        GameObject o;
        //Debug.Log($"pool: {pool}");
        for (int count = pool.Count; count-- > 0;)
        {
            lastIndex = (lastIndex + 1) % pool.Count;
            if (!pool[lastIndex].activeSelf)
            {
                o = pool[lastIndex];
                o.transform.position = newTransform.position;
                o.transform.rotation = newTransform.rotation;
                o.SetActive(true);
                return o;
            }
        }
        o = GameObject.Instantiate(prefab, newTransform.position, newTransform.rotation, parent);
        pool.Add(o);
        return o;
    }

}
