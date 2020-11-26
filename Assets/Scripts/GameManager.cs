using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager> {

    [Header("Players")]
    public PlayerController player;
    public CPUController enemy;

    [Header("UI")]
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;

    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start() {


    }

    // Update is called once per frame
    void Update() {

        if(!isGameOver) {
            if(player.homeBase.isDead) {
                loseText.SetActive(true);
                GameOver();
            }

            if(enemy.homeBase.isDead) {
                winText.SetActive(true);
                GameOver();
            }
        }

    }

    private void GameOver() {
        isGameOver = true;
        Time.timeScale = 0;
    }

}
