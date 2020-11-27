using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Implements a pool for a specific prefab
// Used by PoolManager:  DO NOT USE DIRECTLY

#pragma warning disable 0649
public class Pool
{
    GameObject prefab;
    Transform defaultParent;

    public Pool(GameObject prefab, Transform parent)
    {
        this.prefab = prefab;
        this.defaultParent = parent;
    }

    // Note: msdn documentation claims Lists use an array internally and should perform just as fast
    List<GameObject> pool = new List<GameObject>();
    int lastIndex = -1;

    public GameObject Get(Vector3 position, Quaternion rotation)
    {
        GameObject o;
        //Debug.Log($"pool: {pool}");
        for (int count = pool.Count; count-- > 0;)
        {
            lastIndex = (lastIndex + 1) % pool.Count;
            if (pool[lastIndex] == null)
                return pool[lastIndex] = GameObject.Instantiate(prefab, position, rotation, defaultParent);
            if (!pool[lastIndex].activeSelf)
            {
                o = pool[lastIndex];
                o.transform.position = position;
                o.SetActive(true);
                return o;
            }
        }
        o = GameObject.Instantiate(prefab, position, rotation, defaultParent);
        pool.Add(o);
        return o;
    }

    public GameObject Get(Vector3 position)
    {
        return Get(position, Quaternion.identity);
    }

    // Get and set specified parent
    public GameObject Get(Transform parent)
    {
        GameObject o;
        //Debug.Log($"pool: {pool}");
        for (int count = pool.Count; count-- > 0;)
        {
            lastIndex = (lastIndex + 1) % pool.Count;
            if (!pool[lastIndex].activeSelf)
            {
                o = pool[lastIndex];
                o.transform.parent = parent;
                o.SetActive(true);
                return o;
            }
        }
        o = GameObject.Instantiate(prefab, parent);
        pool.Add(o);
        return o;
    }
    /*
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
    */

}
