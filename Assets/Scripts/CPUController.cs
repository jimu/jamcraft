using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUController : PlayerBase {

    public float enemySpawnRate = 30f;
    private float enemySpawnTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        enemySpawnTimer -= Time.deltaTime;
        if(enemySpawnTimer < 0f) {
            enemySpawnTimer += enemySpawnRate;

            // spawn an enemy
            BotController lastBot = Instantiate(defaultBotPrefab, homeBase.transform.position + (homeBase.transform.forward * 8), homeBase.transform.rotation, null).GetComponent<BotController>();
            bots.Add(lastBot);
            lastBot.Initialize();
            lastBot.SetRally(opponent.homeBase.transform.position);
            lastBot.owner = this;

        }
    }
}
