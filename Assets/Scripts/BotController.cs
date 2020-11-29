using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#pragma warning disable 649
public class BotController : Damageable {

    const float TIMEBETWEENTARGETSEARCHES = 1F;

    public PlayerBase owner;
    private NavMeshAgent navAgent;
    public Vector3 rallyPoint;
    public float radius = 2f;

    [Header("Combat")]
    [SerializeField] private Transform projectilePoint;
    public GameObject projectilePrefab;
    [SerializeField] private Damageable currentTarget;
    [SerializeField]private float range = 0;
    private float timeUntilNextSearch = TIMEBETWEENTARGETSEARCHES;

    public Weapon weapon1;
    public Weapon weapon2;

    public Bot botConfig;

    private Rigidbody rb;

    void Start() {
        if(botConfig != null) LoadBot();
    }

    public void Initialize() {
        Init(); // Damageable
        navAgent = GetComponent<NavMeshAgent>();
        currentTarget = null;

        rb = GetComponent<Rigidbody>();
        if(rb == null) {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }


    }

    public void LoadBot() {
        botConfig = GetComponent<Bot>();
        if(botConfig.weapon1 != null) {
            weapon1 = gameObject.AddComponent<Weapon>();

            weapon1.SetData(botConfig.weapon1);

            if(projectilePoint != null) {
                weapon1.muzzle = projectilePoint;
            } else {
                weapon1.muzzle = botConfig.muzzle1;
            }
            if(weapon1.data.range > range) range = weapon1.data.range;
        }

        if(botConfig.weapon2 != null) {
            weapon2 = gameObject.AddComponent<Weapon>();
            weapon2.SetData(botConfig.weapon2);
            if(projectilePoint != null) {
                weapon2.muzzle = projectilePoint;
            } else {
                weapon2.muzzle = botConfig.muzzle1;
            }
            if(weapon2.data.range > range) range = weapon2.data.range;
        }

        currentHealth = 1f;
        Initialize();

        if(botConfig.chassis != null) {
            navAgent.speed = botConfig.chassis.speed;
            navAgent.angularSpeed = botConfig.chassis.turnSpeed;
            maxHealth = botConfig.chassis.maxHealth;
        }

        

        

    }

    public void SetRally(Vector3 rally) {
        rallyPoint = rally;
        navAgent.SetDestination(rally);
    }

    void Update() {

        if (!isDead)
        {

            

            if (currentTarget != null)
            {

                if(currentTarget.isDead) 
                {
                    currentTarget = null;
                } else { 

                    // disengage if it is too far away
                    if (Vector3.Distance(transform.position, currentTarget.transform.position) + radius > range)
                    {
                        currentTarget = null;

                    }
                    else
                    {

                        navAgent.isStopped = true;

                        // Look at the target
                        Vector3 lookPos = currentTarget.transform.position - transform.position;
                        lookPos.y = 0;
                        Quaternion rotation = Quaternion.LookRotation(lookPos);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 3f * Time.deltaTime);

                        // if the target is infront of the bot, shoot.
                        float angle = 8;
                        if (Vector3.Angle(transform.forward, currentTarget.transform.position - transform.position) < angle)
                        {

                            if(weapon1 != null)
                                if(weapon1.canShoot())
                                    weapon1.Shoot();

                            if(weapon2 != null)
                                if(weapon2.canShoot())
                                    weapon2.Shoot();

                        }
                    }
                }
            }

            timeUntilNextSearch -= Time.deltaTime;
            // if you dont have a target, find one
            if (currentTarget == null && timeUntilNextSearch < 0f)
            {
                timeUntilNextSearch = TIMEBETWEENTARGETSEARCHES;
                if (!FindTarget())
                {
                    navAgent.SetDestination(rallyPoint);
                    return;
                } else {
                    //navAgent.SetDestination(rallyPoint);
                }
            }
        }
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
