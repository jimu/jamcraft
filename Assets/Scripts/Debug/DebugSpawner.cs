using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BotModel
{
    public Transform weaponHardpoint;
}

// DEBUG: Spawns units on keypress 
public class DebugSpawner : MonoBehaviour
{
    public ChassisData chassisData1;
    public ChassisData chassisData2;
    public ChassisData chassisData3;
    public WeaponData weaponData1;
    public WeaponData weaponData2;
    public WeaponData weaponData3;
    Transform navGoal;

    void Start()
    {
        navGoal = GameObject.Find("NavGoal")?.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
            DebugSpawn(chassisData1, weaponData1);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            DebugSpawn(chassisData2, weaponData1);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            DebugSpawn(chassisData2, weaponData2);
        if (Input.GetKeyDown(KeyCode.Alpha9))
            DebugSpawn(chassisData2, weaponData3);
        if (Input.GetKeyDown(KeyCode.Alpha0))
            DebugSpawn(chassisData3, null);
    }

    void DebugSpawn(ChassisData chassisData, WeaponData weaponData)
    {
        Debug.Log($"Debug SpawnBot Pressed: Chassis={chassisData} Weapon={weaponData}");

        GameObject bot = PoolManager.Instance.Get(chassisData.prefab, Vector3.zero);
        if (chassisData.weaponHardpoint != "" && weaponData != null)
        {
            Transform pivot = bot.transform.Find(chassisData.weaponHardpoint);
            GameObject turret = PoolManager.Instance.Get(weaponData.prefab, pivot);
            if (weaponData.fireRate > 0)
                pivot.gameObject.AddComponent<DebugTurretRotator>();
        }
        if (navGoal != null)
        {
            NavMeshAgent agent = bot.GetComponent<NavMeshAgent>();
            agent.SetDestination(navGoal.position);
            agent.speed = chassisData.speed;
            agent.angularSpeed = chassisData.turnSpeed;
        }
    }
}
