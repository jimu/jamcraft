using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour {

    public GameObject homeBase;
    public List<BotController> bots = new List<BotController>();
    public PlayerBase opponent;
    public GameObject defaultBotPrefab;

}
