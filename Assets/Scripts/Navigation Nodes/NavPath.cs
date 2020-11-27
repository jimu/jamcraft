using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A NavPath is a game object with NavNode children
 * It is used by a Navigator to direct NavMeshAgents along the path
 * 
 * A NavPath only knows about it's children
 * 
 */
public class NavPath : MonoBehaviour
{
    [SerializeField] bool loop;

    public int Length
    {
        get { return transform.childCount; }
    }

    public bool IsComplete(int index)
    {
        return !loop && (index < 0 || index >= transform.childCount);
    }

    // GetPosition(n) returns position of node n
    // it automatically loops from its beginning to end
    public Vector3 GetPosition(int index)
    {
        return transform.GetChild(index % transform.childCount).position;
    }
}
