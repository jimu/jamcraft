using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Projectile", menuName = "Jamcraft/Projectile")]
public class ProjectileData : ScriptableObject
{
    [Tooltip("Projectile name")]
    public string projectileName;

    [Tooltip("Prefab used to instantiate this projectile")]
    public GameObject prefab;

    [Tooltip("Particle Effect on impact")]
    public ParticleSystem impactVFX;

    [Tooltip("Sound Effect on impact")]
    public AudioClip impactSFX;

    [Tooltip("Damage this projectile inflicts on target")]
    public float damage;
}
