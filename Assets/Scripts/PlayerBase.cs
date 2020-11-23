using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour {

    public GameObject homeBase;
    public List<BotController> bots = new List<BotController>();
    public PlayerBase opponent;
    public GameObject defaultBotPrefab;


    public void UpdateBots() {

        for(int i = bots.Count - 1; i >= 0; i--) {

            if(bots[i].dead) {
                GameObject bot = bots[i].gameObject;
                bots.RemoveAt(i);
                Destroy(bot);
            }

        }

    }

}
