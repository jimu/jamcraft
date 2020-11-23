using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour {

    public float speed;
    public float distance;
    private PlayerBase owner;

    // Component References
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("Collison detected");
        switch(collision.transform.tag) {

            case "Terrain":
                Destroy(gameObject);
                break;
            case "Enemy":
                Debug.Log("Hit");
                Destroy(gameObject);
                break;
        }

    }

}
