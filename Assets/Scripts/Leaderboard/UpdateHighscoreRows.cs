using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateHighscoreRows : MonoBehaviour
{
    [SerializeField] ScrollRect scrollRect;

    public GameObject rowPrefab;
    [SerializeField] TextMeshProUGUI loadingMessage;
    bool isUpdating = true;

    public void OnEnable()
    {
        Debug.Log("UpdateHighscoreRows.OnEnable()");
        Leaderboard.onChange += UpdateRows;
        UpdateStatus();
    }
    public void OnDisable()
    {
        Debug.Log("UpdateHighscoreRows.OnDisable()");
        Leaderboard.onChange -= UpdateRows;
    }

    private void UpdateStatus()
    {
        loadingMessage.gameObject.SetActive(isUpdating);
    }

    public void UpdateRows(List<Highscore> highscores)
    {
        string playerName = PlayerPrefs.GetString("PlayerName");
        int playerRow = 0;

        Debug.Log("UpdateHighscoreRows.UpdateRows()");
        int i;
        for (i = 0; i < highscores.Count; ++i)
        {
            Highscore highscore = highscores[i];
            GameObject childObject = i < transform.childCount ? transform.GetChild(i).gameObject : Instantiate(rowPrefab, transform);
            HighscoreRow row = childObject.GetComponent<HighscoreRow>();
            bool isPlayer = playerName == highscore.name;
            row.Set(highscore.rank, highscore.name, highscore.score, isPlayer);
            childObject.SetActive(true);
            if (isPlayer)
                playerRow = i;
            
        }
        while (i < highscores.Count)
            transform.GetChild(i).gameObject.SetActive(false);

        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = Mathf.Clamp(1 - (playerRow - 2f) / (i-3), 0, 1);
        //scrollRect.content.anchoredPosition = new Vector2(0, (playerRow * 1.0f + 1)/ i);
        Debug.Log($"Set verticalNormalizedPosition to {scrollRect.verticalNormalizedPosition} {playerRow}/{i}");

        isUpdating = false;
        UpdateStatus();
    }
}
