using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Damageable : MonoBehaviour {


    public enum Alignment { PLAYER, CPU, NEUTRAL }
    public Alignment alignment;
    public float maxHealth;
    public float currentHealth;

    public bool isDead {
        get { return currentHealth <= 0f; }
    }

    public virtual void Init() {
        currentHealth = maxHealth;
    }

    public void Damage(float damage) {
        currentHealth -= damage;
    }

    public float GetNormalizedHealth() {
        return currentHealth / maxHealth;
    }

}
