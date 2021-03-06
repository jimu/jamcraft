using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUController : PlayerBase {

    public float enemySpawnRate = 30f;
    private float enemySpawnTimer = 0f;

    [SerializeField]
    public List<NavNode> startNodes;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawnTimer = enemySpawnRate;
    }

    // Update is called once per frame
    void Update(){

        UpdateBots();

        enemySpawnTimer -= Time.deltaTime;
        if(enemySpawnTimer < 0f) {
            enemySpawnTimer += enemySpawnRate;

            // spawn an enemy
            BotController lastBot = Instantiate(defaultBotPrefab, homeBase.transform.position + (homeBase.transform.forward * 8), homeBase.transform.rotation, null).GetComponent<BotController>();
            bots.Add(lastBot);
            lastBot.Initialize();
            lastBot.SetRally(startNodes[Random.Range(0, startNodes.Count)].GetRandomPointInNode());
            lastBot.owner = this;

        }
    }
}
