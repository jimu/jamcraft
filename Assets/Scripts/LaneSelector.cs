using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 649
public class LaneSelector : MonoBehaviour
{
    [SerializeField] BotDispatcher botDispatcher;
    static readonly int MAX_HOTKEYS = 9;
    int maxHotkeys;

    private void Awake()
    {
        maxHotkeys = Mathf.Min(botDispatcher.navPaths.Length, MAX_HOTKEYS);
    }

    private void Update()
    {
        for (int i = 0; i < maxHotkeys; ++i)
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                botDispatcher.SetFixedLane(botDispatcher.navPaths[i]);
    }

    private void OnGUI()
    {
        GUILayout.BeginArea((new Rect(Screen.width - 300 , 10, 100, 900)));
        GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        foreach (NavPath lane in botDispatcher.navPaths)
            if (GUILayout.Button($"{lane.name}"))
                botDispatcher.SetFixedLane(lane);

        GUILayout.BeginVertical();
        GUILayout.EndArea();

    }
}
