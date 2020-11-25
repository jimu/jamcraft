using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NavNode : MonoBehaviour {

    public NavNode nextNodeTowardPlayer;
    public NavNode nextNodeTowardCPU;

    private void Start() {
        Debug.Log(GetRandomPointInNode());
    }

    public Vector3 GetRandomPointInNode() {

        Collider collider = GetComponent<Collider>();
        Vector3 center = collider.bounds.center;
        Vector3 offset = new Vector3( Random.Range(-collider.bounds.extents.x, collider.bounds.extents.x), 0, Random.Range(-collider.bounds.extents.z, collider.bounds.extents.z));

        return center + offset;


    }

    public void OnTriggerEnter(Collider other) {

        var bot = other.gameObject.GetComponent<BotController>();
        if(bot != null) {
            if(bot.alignment == BotController.Alignment.CPU) {
                bot.SetRally(nextNodeTowardPlayer.GetRandomPointInNode());
            } else {
                bot.SetRally(nextNodeTowardPlayer.GetRandomPointInNode());
            }
        }
    }


}
