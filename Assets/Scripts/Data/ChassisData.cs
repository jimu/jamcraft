using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Chassis", menuName = "Jamcraft/Chassis")]
public class ChassisData : CraftableData
{
    [Header("Chassis Specification")]
    [Tooltip("Chassis name")]
    public string locomotionName;

    [Tooltip("Prefab used to instantiate bot chassis")]
    public GameObject prefab;

    [Tooltip("Weapon Hardpoint (Child GameObject name)")]
    public string weaponHardpoint;

    [Tooltip("Weapon Hardpoint (Child GameObject name)")]
    public string weaponHardpoint2;

    [Tooltip("Armour Hardpoint (Child GameObject name)")]
    public string armourHardpoint;

    [Tooltip("Movement speed")]
    public float speed = 3;

    [Tooltip("Bot's angular turn speed when changing direction")]
    public float turnSpeed = 60;

    [Tooltip("Bot's health")]
    public float maxHealth = 20;

}
