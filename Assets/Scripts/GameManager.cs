using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager> {

    [Header("Players")]
    public PlayerController player;
    public PlayerBase enemy;

    // Start is called before the first frame update
    void Start() {
        player.opponent = enemy;
        enemy.opponent = player;
    }

    // Update is called once per frame
    void Update() {

        


    }



}
