using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Alignment { }

public class Bot : MonoBehaviour
{
    public ChassisData chassis;
    public WeaponData weapon1;
    public WeaponData weapon2;
    public ArmourData armour;
    public PlayerBase owner;
    public Transform muzzle1;
    public Transform muzzle2;
}
