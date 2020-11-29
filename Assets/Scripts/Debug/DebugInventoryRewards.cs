using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 649

public class DebugInventoryRewards : MonoBehaviour
{
    [Header("Single items to add the inventory")]
    [SerializeField] CraftableData[] singleRewards;

    [Header("List of items to add the inventory at once")]
    [SerializeField] CraftableData[] multipleReward;

    private void OnGUI()
    {
        GUILayout.BeginArea((new Rect(Screen.width - 125, 10, 115, 900)));
        GUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        foreach (var reward in singleRewards)
            if (reward != null)
                if (GUILayout.Button($"{reward.name}"))
                    SendReward(reward);

        if (multipleReward.Length > 0)
            if (GUILayout.Button("MultiReward"))
                SendReward(multipleReward);

        GUILayout.BeginVertical();
        GUILayout.EndArea();

    }

    void SendReward(CraftableData reward)
    {
        Debug.Log($"Sending {reward.name}!");
        Crafter.Instance.AddToInventory(reward);
    }
    void SendReward(CraftableData[] rewards)
    {
        Debug.Log($"Sending Multi-Reward!");
        Crafter.Instance.AddToInventory(rewards);
    }

}
