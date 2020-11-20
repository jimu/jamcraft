using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Recipe
{
    public int redBricks;
    public int blueBricks;
    public int greenBricks;
    public int yellowBricks;
}


[CreateAssetMenu(fileName = "New Weapon", menuName = "Jamcraft/Weapon")]
public class WeaponData : ScriptableObject
{
    [Tooltip("Weapon name")]
    public string weaponName;

    [Tooltip("Prefab used to instantiate this weapon")]
    public GameObject prefab;

    [Tooltip("Projectile that this weapon fires")]
    public ProjectileData projectile;

    [Tooltip("Rounds per second")]
    public float fireRate;

    [Tooltip("Particle Effect when fired")]
    public ParticleSystem fireVFX;

    [Tooltip("Sound Effect when fired")]
    public AudioClip fireSFX;

    [Tooltip("Recipe")]
    public Recipe recipe;

    //[Tooltip("TBD Cost to upgrade this weapon")]
    //public int upgradeCost;

    //[Tooltip("TBD Upgrades to this weapon")]
    //public WeaponData upgrade;

    //[Tooltip("TBD Cost to upgrade this weapon")]
    //public int upgradeCost;
}
