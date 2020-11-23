using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour {


    public PlayerBase owner;
    private NavMeshAgent navAgent;
    private Vector3 rallyPoint;
    public float radius = 2f;

    [Header("Combat")]
    [SerializeField] private Transform projectilePoint;
    public GameObject projectilePrefab;
    private Transform currentTarget;
    public float range = 60f;
    private float attackTimer = 0f;
    public float attackRate = 1f;
    

    void Start() {
        
    }

    public void Initialize() {
        navAgent = GetComponent<NavMeshAgent>();
        currentTarget = null;
    }

    public void SetRally(Vector3 rally) {
        rallyPoint = rally;
        navAgent.SetDestination(rally);
    }

    void Update() {

        attackTimer -= Time.deltaTime;

        if(currentTarget != null) {

            Vector3 lookPos = currentTarget.transform.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2f * Time.deltaTime);

            float angle = 5;
            if(Vector3.Angle(transform.forward, currentTarget.position - transform.position) < angle) {
                if(attackTimer < 0f) Shoot();
            }
        }

        if(currentTarget == null) {
            if(!FindTarget()) {
                navAgent.SetDestination(rallyPoint);
                return;
            }
        }

    }

    private void Shoot() {
        attackTimer = attackRate;
        if(projectilePrefab == null) return;
        Instantiate(projectilePrefab, projectilePoint.position, transform.rotation, null);

    }


    private bool FindTarget() {

        foreach(BotController opponentBot in owner.opponent.bots) {
            if(Vector3.Distance(transform.position, opponentBot.transform.position) + radius <= range) {
                currentTarget = opponentBot.transform;
                navAgent.isStopped = true;
                return true;
            }
        }

        return false;
    }

}
