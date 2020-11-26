using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : Damageable {

    public PlayerBase owner;
    private NavMeshAgent navAgent;
    public Vector3 rallyPoint;
    public float radius = 2f;

    [Header("Combat")]
    [SerializeField] private Transform projectilePoint;
    public GameObject projectilePrefab;
    private Damageable currentTarget;
    public float range = 60f;
    private float attackTimer = 0f;
    public float attackRate = 1f;

    void Start() {
        
    }

    public void Initialize() {
        Init(); // Damageable
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

            // disengage if it is too far away
            if(Vector3.Distance(transform.position, currentTarget.transform.position) + radius > range) {
                currentTarget = null;
                
            } else{

                navAgent.isStopped = true;

                // Look at the target
                Vector3 lookPos = currentTarget.transform.position - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2f * Time.deltaTime);

                // if the target is infront of the bot, shoot.
                float angle = 8;
                if(Vector3.Angle(transform.forward, currentTarget.transform.position - transform.position) < angle) {
                    if(attackTimer < 0f) Shoot();
                }
            }
        }

        // if you dont have a target, find one
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

        if(Vector3.Distance(transform.position, owner.opponent.homeBase.transform.position) + radius <= range) {
            currentTarget = owner.opponent.homeBase;
        } else {
            foreach(BotController opponentBot in owner.opponent.bots) {

                if(Vector3.Distance(transform.position, opponentBot.transform.position) + radius <= range) {
                    currentTarget = opponentBot;
                    navAgent.isStopped = true;
                    return true;
                }
            }
        }

        return false;
    }

}
