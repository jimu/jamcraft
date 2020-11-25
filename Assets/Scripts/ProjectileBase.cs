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
    }

    private void OnCollisionEnter(Collision collision) {

        Damageable damageable = collision.transform.GetComponent<Damageable>();

        if(damageable != null) {
            damageable.Damage(damage);
        }

        Destroy(gameObject);

    }

}
