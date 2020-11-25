using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armour", menuName = "Jamcraft/Armour")]
public class ArmourData : ScriptableObject {
    [Tooltip("Armour name")]
    public string armourName;

    [Tooltip("Prefab used to instantiate bot chassis")]
    public GameObject prefab;

    [Tooltip("Armour Provided")]
    public float damageReduction;

    [Tooltip("Purchase Cost")]
    public Recipe recipe;
}
