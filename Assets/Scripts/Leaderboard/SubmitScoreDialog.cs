using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
#pragma warning disable 649
public class SubmitScoreDialog : MonoBehaviour
{
    [SerializeField] GameObject highScoresPanel;
//    [SerializeField] TextMeshProUGUI nameInputField;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TMP_InputField inputField;
    private int score;
    
    public void Activate(int score)
    {
        this.score = score;
        inputField.text = PlayerPrefs.GetString("PlayerName");
        Debug.Log($"Activate: nameInputField.text={inputField.text} pp={PlayerPrefs.GetString("PlayerName")}");
        scoreText.text = score.ToString();
        gameObject.SetActive(true);
        inputField.Select();
        inputField.ActivateInputField();
    }

    public void OnSubmitPressed()
    {
        string name = inputField.text;
        PlayerPrefs.SetString("PlayerName", name);
        PlayerPrefs.Save();
        Leaderboard.Instance.GetComponent<SubmitScore>().Submit(name, score);

        highScoresPanel.SetActive(true);
        gameObject.SetActive(false);
    }

}
