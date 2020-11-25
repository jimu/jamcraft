using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Chassis", menuName = "Jamcraft/Chassis")]
public class ChassisData : ScriptableObject
{
    [Tooltip("Chassis name")]
    public string locomotionName;

    [Tooltip("Prefab used to instantiate bot chassis")]
    public GameObject prefab;

    [Tooltip("Turret Hardpoint (Child GameObject name)")]
    public string turretHardpoint;

    [Tooltip("Movement speed")]
    public float speed = 3;

    [Tooltip("Bot's angular turn speed when changing direction")]
    public float turnSpeed = 60;

    [Tooltip("Purchase Cost")]
    public Recipe recipe;
}
