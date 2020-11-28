using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 649
public class LaneSelector : MonoBehaviour
{
    [SerializeField] BotDispatcher botDispatcher;

    private void OnGUI()
    {
        GUILayout.BeginArea((new Rect(Screen.width / 2 - 50 , 10, 100, 900)));
        GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        foreach (NavPath lane in botDispatcher.navPaths)
            if (GUILayout.Button($"{lane.name}"))
                botDispatcher.SetFixedLane(lane);

        GUILayout.BeginVertical();
        GUILayout.EndArea();

    }
}
