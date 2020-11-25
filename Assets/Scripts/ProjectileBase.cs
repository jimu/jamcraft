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
        Debug.Log("Collison detected");
        switch(collision.transform.tag) {

            case "Terrain":
                Destroy(gameObject);
                break;
            case "Enemy":
                BotController bot = collision.transform.GetComponent<BotController>();
                bot.Damage(damage);
                Destroy(gameObject);
                break;
        }

    }

}
