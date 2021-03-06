using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Damageable : MonoBehaviour {


    public enum Alignment { PLAYER, CPU, NEUTRAL }
    public Alignment alignment;
    public float maxHealth;
    public float currentHealth = 1f;
    public GameObject explodeTrigger;

    public bool isDead {
        get { return currentHealth <= 0f; }
    }

    public virtual void Init() {
        currentHealth = maxHealth;
    }

    public void Damage(float damage) {
        currentHealth -= damage;
        if (isDead)
            Explode();
    }

    public float GetNormalizedHealth() {
        if(currentHealth <= 0) return 0.001f;
        return currentHealth / maxHealth;
    }

    public void Explode()
    {
        if (explodeTrigger != null)
            explodeTrigger.SetActive(true);
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        //if (agent)
        //    agent.isStopped = true;
        gameObject.SetActive(false);
    }
}
