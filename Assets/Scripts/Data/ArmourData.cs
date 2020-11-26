using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armour", menuName = "Jamcraft/Armour")]
public class ArmourData : CraftableData
{
    [Header("Armour Specification")]

    [Tooltip("Armour name")]
    public string armourName;

    [Tooltip("Prefab used to instantiate bot chassis")]
    public GameObject prefab;

    [Tooltip("Armour Provided")]
    public float damageReduction;
}
