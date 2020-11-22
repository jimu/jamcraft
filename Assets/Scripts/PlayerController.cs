using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour {

    public List<BotController> bots;
    public InventorySystem inventory;
    public GameObject rallyPoint;
    public Camera cam;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if(Input.GetMouseButtonDown(0)) {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)) {

                rallyPoint.transform.position = hit.point; 

                foreach(BotController bot in bots) {
                    bot.SetRally(hit.point);
                }

            }

        }

    }

    public void RallyBots() {

    }

}
