using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Locomotion", menuName = "Jamcraft/Locomotion")]
public class LocomotionData : ScriptableObject
{
    [Tooltip("Locomotion name")]
    public string locomotionName;

    [Tooltip("Prefab used to instantiate bot locomotion/base")]
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
