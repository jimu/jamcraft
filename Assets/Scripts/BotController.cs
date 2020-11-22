using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour {

    private NavMeshAgent navAgent;
    private Vector3 rallyPoint;

    void Start() {
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void SetRally(Vector3 rally) {
        rallyPoint = rally;
        navAgent.SetDestination(rally);
    }

    void Update() {
        
    }
}
