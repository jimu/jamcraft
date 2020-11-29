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
                Debug.Log(bots[i].transform.name + " Died");
                GameObject bot = bots[i].gameObject;

                // GIVE PLAYER RESOURCE
                if(bots[i].alignment == BotController.Alignment.CPU) {
                    if(GameManager.Instance.possibleLoot.Length > 0) {
                        Crafter.Instance.AddToInventory(GameManager.Instance.possibleLoot[Random.Range(0, GameManager.Instance.possibleLoot.Length-1)]);
                    }
                }

                bots.RemoveAt(i);
                //Destroy(bot);   explode it instead and then wait 10 seconds before destroying the exploded bot
            }

        }

    }

}
