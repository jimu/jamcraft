using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 649
public class LaneSelector : MonoBehaviour
{
    [SerializeField] BotDispatcher botDispatcher;

    [Tooltip("Prefab to load from the Resources folder")]
    [SerializeField] string prefabResource = "LaneIndicator";
    [SerializeField] float indicatorSpeed = 20f;
    LaneIndicator laneIndicator;

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
                SetLane(i);
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 300 , 10, 100, 900));
        GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        for (int i = 0; i < botDispatcher.navPaths.Length; ++i)
            foreach (NavPath lane in botDispatcher.navPaths)
                if (GUILayout.Button($"{botDispatcher.navPaths[i].name}"))
                    SetLane(i);

        GUILayout.BeginVertical();
        GUILayout.EndArea();
    }

    void InitLaneIndicator()
    {
        if (laneIndicator == null)
            laneIndicator = Instantiate(Resources.Load<LaneIndicator>(prefabResource));
        laneIndicator.speed = indicatorSpeed;
    }

    private void SetLane(int n)
    {
        InitLaneIndicator();

        Vector3 point1 = botDispatcher.navPaths[n].GetPosition(0);
        Vector3 point2 = botDispatcher.navPaths[n].GetPosition(1);
        laneIndicator.SetPositions(point1, point2);

        botDispatcher.SetFixedLane(botDispatcher.navPaths[n]);
    }
}
