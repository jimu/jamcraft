using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour {

    public float speed;
    public float distance;
    private PlayerBase owner;
    private int damage = 1;
    private ProjectileData data;
     
    // Component References
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;
    }

    public void SetData(ProjectileData data)
    {
        this.data = data;
    }

    // Update is called once per frame
    void Update() {
        distance -= Time.deltaTime * speed;
        if(distance <= 0f) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {

        Damageable damageable = collision.transform.GetComponent<Damageable>();

        if(damageable != null &&
            (damageable.alignment == Damageable.Alignment.PLAYER && CompareTag("Projectile-Enemy") ||
            damageable.alignment == Damageable.Alignment.CPU && CompareTag("Projectile-Friendly")))
        {
            damageable.Damage(data.damage);
            //Debug.Log($" {damageable.name} hit by {name} hits={damageable.currentHealth}/{damageable.maxHealth}");

            Debug.Assert(data != null, $"projectile data for {name} is null", this);
            Debug.Assert(data.impactSFX != null, $"data {data.name} impact sfx is null", this);

            AudioManager.Instance.PlayOneShot(data.impactSFX);

            if (CompareTag("Projectile-Friendly"))
            {
                gameObject.SetActive(false);

            }
            else
                Destroy(gameObject);
        }


    }

}
