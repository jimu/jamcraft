using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour {

    public HomeBase homeBase;
    public List<BotController> bots = new List<BotController>();
    public PlayerBase opponent;
    public GameObject defaultBotPrefab;


    public void UpdateBots() {

        for(int i = bots.Count - 1; i >= 0; i--) {

            if(bots[i].isDead) {
                GameObject bot = bots[i].gameObject;
                bots.RemoveAt(i);
                //Destroy(bot);   explode it instead and then wait 10 seconds before destroying the exploded bot
            }

        }

    }

}
