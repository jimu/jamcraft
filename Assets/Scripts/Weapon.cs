using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 640

// fires projectiles
public class Weapon : MonoBehaviour{

    public Transform muzzle;

    [Tooltip("Data should be set programatically")]
    public WeaponData data;

    private float attackCooldown;
    private float attackTimer;

    private void Start() {
        attackCooldown = 1f / data.fireRate;
        attackTimer = 0f;
    }

    public void SetData(WeaponData data)
    {
        this.data = data;
        attackCooldown = 1f / data.fireRate;
        attackTimer = 0f;
    }

    public void Shoot() {
        attackTimer = attackCooldown;
        //PoolManager.Instance.Get(data.projectile.prefab, muzzle.position);
        Instantiate(data.projectile.prefab, muzzle.position, muzzle.transform.rotation, null);

    }

    public void Update() {
        attackTimer -= Time.deltaTime;
    }

    public bool canShoot() {
        return attackTimer <= 0f;
    }

}
