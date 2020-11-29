using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneIndicator : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    Vector3 startPosition, endPosition;
    float normalizedSpeed;
    
    public void SetPositions(Vector3 start, Vector3 end)
    {
        gameObject.SetActive(true);
        startPosition = start;
        endPosition = end;
        normalizedSpeed = speed / Vector3.Distance(end, start);
    }

    //  time  speed distance
    //  0     1     5
    //  1     1     5
    void Update()
    {
        transform.position = Vector3.Lerp(startPosition, endPosition, normalizedSpeed * Time.time % 1);
    }
}
