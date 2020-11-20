using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#pragma warning disable 649
public class RandomScoreButton : MonoBehaviour
{
    [SerializeField] SubmitScoreDialog submitScoreDialog;
    [SerializeField] GameObject highScoresPanel;


    public void OnPressed()
    {
        highScoresPanel.SetActive(false);
        submitScoreDialog.Activate(Random.Range(1, 1000));
    }

}
