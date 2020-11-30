using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Targeter : MonoBehaviour
{
    Damageable currentTarget = null;
    Bot bot;
    float disengageRadius = 2f;
    float fireReadyTime1 = 0f;
    float fireReadyTime2 = 0f;
    float searchReadyTime = 0f;
    NavMeshAgent navAgent;


    void Start()
    {
        bot = GetComponent<Bot>();
        navAgent = GetComponent<NavMeshAgent>();
    }


    // Update is called once per frame
    void Update()
    {
        if (IsEngaged())
        {
            if (Vector3.Distance(transform.position, currentTarget.transform.position) + disengageRadius > bot.weapon1.range)
            {
                Disengage();
            }
            else
            {
                Vector3 lookPos = currentTarget.transform.position - transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2f * Time.deltaTime);
                // if the target is infront of the bot, shoot.
                float angle = 8;
                if (Vector3.Angle(transform.forward, currentTarget.transform.position - transform.position) < angle)
                    Shoot();
            }
        }

        if (!IsEngaged())
            FindTarget();
    }


    private void Shoot()
    {
        if (bot.weapon1 && Time.time > fireReadyTime1)
        {
            fireReadyTime1 = Time.time + 1f / bot.weapon1.fireRate;
            if (bot.weapon1.projectile.prefab == null)
                return;

            Debug.Log($"{name} {transform.position} {transform.rotation.eulerAngles}: shooting at {currentTarget.name} ({currentTarget.transform.position})");

            GameObject g = PoolManager.Instance.Get(bot.weapon1.projectile.prefab, bot.muzzle1.position, bot.muzzle1.rotation);
            AudioManager.Instance.PlayOneShot(bot.weapon1.fireSFX);
            //Debug.Log($" * firer      {name}: ({transform.position}) {transform.rotation.eulerAngles}");
            //Debug.Log($" * projectile {g.name}: ({g.transform.position}) {g.transform.rotation.eulerAngles}");
        }

        if (bot.weapon2 && Time.time > fireReadyTime2)
        {
            fireReadyTime2 = Time.time + 1f / bot.weapon2.fireRate;
            if (bot.weapon2.projectile.prefab == null)
                return;

            PoolManager.Instance.Get(bot.weapon2.projectile.prefab, transform.position, transform.rotation);
        }
    }


    public bool IsEngaged()
    {
        return currentTarget != null;
    }

    public void Engage(Damageable target)
    {
        currentTarget = target;
        navAgent.isStopped = true;
        fireReadyTime2 = fireReadyTime1 + 0.5f / bot.weapon1.fireRate;
    }

    public void Disengage()
    {
        currentTarget = null;
        navAgent.isStopped = false;
    }

    private void FindTarget()
    {
        if (Time.time > searchReadyTime)
        {
            searchReadyTime = Time.time + 1f;
            if (Vector3.Distance(transform.position, bot.owner.opponent.homeBase.transform.position) + disengageRadius <= bot.weapon1.range)
                Engage(bot.owner.opponent.homeBase);
            else
                foreach (BotController opponentBot in bot.owner.opponent.bots)
                    if (Vector3.Distance(transform.position, opponentBot.transform.position) + disengageRadius <= bot.weapon1.range)
                        Engage(opponentBot);
        }
    }
}

