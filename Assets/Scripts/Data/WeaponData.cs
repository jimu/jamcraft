using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Weapon", menuName = "Jamcraft/Weapon")]
public class WeaponData : CraftableData
{
    [Header("Weapon Specification")]

    [Tooltip("Weapon name")]
    public string weaponName;

    [Tooltip("Prefab used to instantiate this weapon")]
    public GameObject prefab;

    [Tooltip("Projectile that this weapon fires")]
    public ProjectileData projectile;

    [Tooltip("Rounds per second")]
    public float fireRate;

    [Tooltip("Weapon range")]
    public float range;

    [Tooltip("Particle Effect when fired")]
    public ParticleSystem fireVFX;

    [Tooltip("Sound Effect when fired")]
    public AudioClip fireSFX;

    //[Tooltip("TBD Cost to upgrade this weapon")]
    //public int upgradeCost;

    //[Tooltip("TBD Upgrades to this weapon")]
    //public WeaponData upgrade;

    //[Tooltip("TBD Cost to upgrade this weapon")]
    //public int upgradeCost;
}
