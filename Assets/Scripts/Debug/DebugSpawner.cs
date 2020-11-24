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
    public LocomotionData locoData1;
    public LocomotionData locoData2;
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
            DebugSpawn(locoData1, weaponData1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            DebugSpawn(locoData2, weaponData1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            DebugSpawn(locoData2, weaponData2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            DebugSpawn(locoData2, weaponData3);
    }

    void DebugSpawn(LocomotionData locomotionData, WeaponData weaponData)
    {
        Debug.Log($"Debug SpawnBot Pressed: Locomotion={locomotionData} Weapon={weaponData}");

        GameObject bot = PoolManager.Instance.Get(locomotionData.prefab, Vector3.zero);
        if (locomotionData.turretHardpoint != "")
        {
            Transform pivot = bot.transform.Find(locomotionData.turretHardpoint);
            GameObject turret = PoolManager.Instance.Get(weaponData.prefab, pivot);
            if (weaponData.fireRate > 0)
                pivot.gameObject.AddComponent<DebugTurretRotator>();
        }
        if (navGoal != null)
        {
            NavMeshAgent agent = bot.GetComponent<NavMeshAgent>();
            agent.SetDestination(navGoal.position);
            agent.speed = locomotionData.speed;
            agent.angularSpeed = locomotionData.turnSpeed;
        }
    }

}
