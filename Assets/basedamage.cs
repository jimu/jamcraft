using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basedamage : MonoBehaviour
{ 
    public ParticleSystem ps1;
    public ParticleSystem ps2;
    public ParticleSystem ps3;
    public int currentHealth;
    // nonExplosive objects are the ones the LEGO explosive brick won't destroy because they're parented elsewhere or aren't LEGO
    public GameObject nonExplosive;

    // Start is called before the first frame update
    void Start()
    {
        //pause the particle systems and set current base health for testing
        ps1.Pause();
        ps2.Pause();
        ps3.Pause();
        currentHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth > 79)
        {
            print("im ok");
        }
        else if(currentHealth > 60)
        {
            ps1.Play();
            print("one");
        }
        else if(currentHealth > 20)
        {
            ps1.Play();
            ps2.Play();
            print("two");
        }
        else if (currentHealth > 0)
        {
            ps1.Play();
            ps2.Play();
            ps3.Play();
            print("three");
        }
        else
        {
            print("how did we get here?");
            Destroy(nonExplosive);
            // insert lego explode script here
        }

    }
}
