using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCheats : MonoBehaviour
{
   // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
            GameManager.Instance.Lose();
        if (Input.GetKeyDown(KeyCode.RightBracket))
            GameManager.Instance.Win();
        if (Input.GetKeyDown(KeyCode.Backslash))
            DumpStats();
    }

    void DumpStats()
    {
        Debug.Log($"Time Scale: {Time.timeScale}");
    }
}

