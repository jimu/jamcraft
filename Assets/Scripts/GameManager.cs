using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


enum GameState { Error, Start, Play, Pause, Help, Win, Lose };

#pragma warning disable 0649,414
public class GameManager : MonoSingleton<GameManager> {

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Players")]
    public PlayerController player;
    public CPUController enemy;

    [Header("UI")]
    [SerializeField] private GameObject winText; // TODO delete
    [SerializeField] private GameObject loseText; // TODO delete
    [SerializeField] float loseDelay = 2f;
    [SerializeField] float winDelay = 4f;

    public CraftableData[] possibleLoot;

    private GameState state = GameState.Error;


    private void Start()
    {
        SetGameState(GameState.Play);
    }


    // Update is called once per frame
    void Update() {

        if(state == GameState.Play) {

            if (enemy.homeBase.isDead)
                Win();

            if (player.homeBase.isDead)
                Lose();
        }

    }

    void SetGameState(GameState state)
    {
        this.state = state;
        Time.timeScale = (state == GameState.Play || state == GameState.Lose || state == GameState.Win) ? 1 : 0;
    }


    public void Win()
    {
        Debug.Log("Win()");
        SetGameState(GameState.Win);

        // Wait for explosions / FX
        // TODO - calculate score and HighScores scene instead?   Preserve score and load next scene?
        StartCoroutine(DelayedSceneLoad("Menu Win", winDelay));
    }

    public void Lose()
    {
        Debug.Log("Lose()");

        SetGameState(GameState.Lose);

        // Wait for explosions / FX
        // TODO - calculate cumulative score from all levels and load HighScores scene instead?
        StartCoroutine(DelayedSceneLoad("Menu Lose", winDelay));

    }


    public IEnumerator DelayedSceneLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
