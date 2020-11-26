using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBotManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Crafter.Instance.onNewBot += InitNewBot;
    }

    private void OnDisable()
    {
        Crafter.Instance.onNewBot -= InitNewBot;

    }

    void InitNewBot(GameObject bot)
    {
        Debug.Log($"InitNewBot({bot.name})");
    }
}
