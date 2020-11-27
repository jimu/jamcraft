using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 640

// fires projectiles
public class Weapon : MonoBehaviour
{
    public Transform muzzle;

    [Tooltip("Data should be set programatically")]
    public WeaponData data;

    public void SetData(WeaponData data)
    {
        this.data = data;
    }

        
}
