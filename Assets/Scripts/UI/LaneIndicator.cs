using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Repeatedly moves game object along first leg of a lane to indicate selected lane
 * 
 * This object should not be activated unless or until a lane has been selected
 */
public class LaneIndicator : MonoBehaviour
{
    [SerializeField] public float speed = 20f;
    Vector3 startPosition, endPosition;
    float normalizedSpeed;
    
    // set path and enable game object
    public void SetPositions(Vector3 start, Vector3 end)
    {
        gameObject.SetActive(true);
        startPosition = start;
        endPosition = end;
        normalizedSpeed = speed / Vector3.Distance(end, start);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(startPosition, endPosition, normalizedSpeed * Time.time % 1);
    }
}
