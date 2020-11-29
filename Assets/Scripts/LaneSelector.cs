using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 649
public class LaneSelector : MonoBehaviour
{
    [SerializeField] BotDispatcher botDispatcher;

    [Tooltip("World GameObject that indicates the currently selected lane")]
    [SerializeField] LaneIndicator laneIndicator;

    static readonly int MAX_HOTKEYS = 9;
    int maxHotkeys;

    private void Awake()
    {
        maxHotkeys = Mathf.Min(botDispatcher.firstNodes.Length, MAX_HOTKEYS);
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

        for(int i = 0; i < botDispatcher.firstNodes.Length; i++)
            if (GUILayout.Button($"{botDispatcher.firstNodes[i].transform.parent.name}"))
                SetLane(i);

        GUILayout.BeginVertical();
        GUILayout.EndArea();
    }


    private void SetLane(int n)
    {
        Vector3 point1 = botDispatcher.firstNodes[n].transform.position;
        Vector3 point2 = botDispatcher.firstNodes[n].nextNodeTowardCPU.transform.position;
        laneIndicator.SetPositions(point1, point2);

        botDispatcher.SetFixedLane(n);
    }
}
