using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour {


    public PlayerBase owner;
    private NavMeshAgent navAgent;
    private Vector3 rallyPoint;
    public float radius = 2f;
    public float range = 6f;

    public BotController currentTarget;

    void Start() {
        
    }

    public void Initialize() {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void SetRally(Vector3 rally) {
        rallyPoint = rally;
        navAgent.SetDestination(rally);
    }

    void Update() {

        if(currentTarget == null && navAgent.isStopped) {
            navAgent.SetDestination(rallyPoint);
            return;
        }

    }

    private bool FindTargets() {

        foreach(BotController opponentBot in owner.opponent.bots) {
            if(Vector3.Distance(transform.position, opponentBot.transform.position) + radius < range) {
                currentTarget = opponentBot;
                return true;
            }
        }

        return false;
    }

}
