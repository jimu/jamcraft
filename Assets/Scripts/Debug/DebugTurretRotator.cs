using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Rotates a bot's turret(s) continually
public class DebugTurretRotator : MonoBehaviour
{
    [SerializeField] float speed = 45f;

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
