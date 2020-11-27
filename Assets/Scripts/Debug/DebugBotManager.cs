using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugBotManager : MonoBehaviour
{
    Transform navGoal;
    void Start()
    {
        Crafter.Instance.onNewBot += InitNewBot;
        navGoal = GameObject.Find("NavGoal")?.transform;

    }

    private void OnDisable()
    {
        Crafter.Instance.onNewBot -= InitNewBot;

    }

    void InitNewBot(Bot bot)
    {
        Debug.Log($"InitNewBot({bot.name})");
        Bot botdata = bot.GetComponent<Bot>();

        if (navGoal != null)
        {
            NavMeshAgent agent = bot.GetComponent<NavMeshAgent>();
            agent.SetDestination(navGoal.position);
            agent.speed = botdata.chassis.speed;
            agent.angularSpeed = botdata.chassis.turnSpeed;
        }

    }
}
