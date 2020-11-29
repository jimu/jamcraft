using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour {

    public float speed;
    public float distance;
    private PlayerBase owner;
    public int damage = 1;
     
    // Component References
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update() {
        distance -= Time.deltaTime * speed;
        if(distance <= 0f) Destroy(gameObject);

        //if (CompareTag("Projectile-Friendly"))
        //{
        //    transform.Translate(speed * Vector3.forward * Time.deltaTime);
        //}
    }

    private void OnCollisionEnter(Collision collision) {

        Damageable damageable = collision.transform.GetComponent<Damageable>();

        if(damageable != null &&
            (damageable.alignment == Damageable.Alignment.PLAYER && CompareTag("Projectile-Enemy") ||
            damageable.alignment == Damageable.Alignment.CPU && CompareTag("Projectile-Friendly")))
        {
            damageable.Damage(damage);
            //Debug.Log($" {damageable.name} hit by {name} hits={damageable.currentHealth}/{damageable.maxHealth}");

            if (CompareTag("Projectile-Friendly"))
            {
                gameObject.SetActive(false);

            }
            else
                Destroy(gameObject);
        }


    }

}
