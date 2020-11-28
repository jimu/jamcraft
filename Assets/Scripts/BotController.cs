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
    private Damageable currentTarget;
    private float range;
    private float timeUntilNextSearch = TIMEBETWEENTARGETSEARCHES;

    public Weapon weapon1;
    public Weapon weapon2;

    public Bot botConfig;

    void Start() {

    }

    public void Initialize() {
        Init(); // Damageable
        navAgent = GetComponent<NavMeshAgent>();
        currentTarget = null;
    }

    public void LoadBot() {
        botConfig = GetComponent<Bot>();
//        weapon1.SetData(botConfig.weapon1);
//        weapon1.muzzle = botConfig.muzzle1;
//        weapon2.SetData(botConfig.weapon2);
//        weapon2.muzzle = botConfig.muzzle2;

//        range = Mathf.Max(weapon1 != null ? weapon1.data.range : 0f, weapon2 != null ? weapon2.data.range : 0f);

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
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2f * Time.deltaTime);

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

            // if you dont have a target, find one
            if (currentTarget == null && timeUntilNextSearch < 0f)
            {
                timeUntilNextSearch = TIMEBETWEENTARGETSEARCHES;
                if (!FindTarget())
                {
                    navAgent.SetDestination(rallyPoint);
                    return;
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
