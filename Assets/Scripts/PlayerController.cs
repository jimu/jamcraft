using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : PlayerBase {

    public Camera cam;
    public GameObject rallyPoint;

    // Start is called before the first frame update
    void Start() {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {

        // Left click to rappy bots
        if(Input.GetMouseButtonDown(0)) {
            RallyBots();
        }

        // testing building a bot
        if(Input.GetKeyDown("1")) {
            BotController lastBot = Instantiate(defaultBotPrefab, homeBase.transform.position + (homeBase.transform.forward * 8), homeBase.transform.rotation, null).GetComponent<BotController>();
            bots.Add(lastBot);
            lastBot.Initialize();
            lastBot.SetRally(GetRallyPoint());
            lastBot.owner = this;
        }

    }

    public Vector3 GetRallyPoint() {
        return rallyPoint.transform.position;
    }

    public void RallyBots() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit)) {

            if(hit.collider.tag == "Ground") {

                rallyPoint.transform.position = hit.point;

                foreach(BotController bot in bots) {
                    bot.SetRally(hit.point);
                }
            }
        }
    }

}
