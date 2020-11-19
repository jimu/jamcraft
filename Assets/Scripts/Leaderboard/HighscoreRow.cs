using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#pragma warning disable 649
public class HighscoreRow : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI rank;
    public new TextMeshProUGUI name;
    public TextMeshProUGUI score;
    public Color defaultColor = new Color32(0xDA, 0xFF, 0xB8, 0xFF);
    public Color highlightColor = new Color32(0xFF, 0xA7, 0x23, 0xFF);
    static readonly Color defaultTextColor = Color.white;
    static readonly Color highlightTextColor = Color.yellow;

    [SerializeField] Sprite leaderIcon;
    [SerializeField] Sprite playerIcon;
    [SerializeField] Sprite currentPlayerIcon;

    public void Set(int rank, string name, int score, bool isPlayer = false)
    {
        icon.sprite = rank == 1 ? leaderIcon : isPlayer ? currentPlayerIcon : playerIcon;
        this.rank.text = rank.ToString();
        this.name.text = name;
        this.name.color = isPlayer ? highlightTextColor : defaultTextColor;
        this.score.text = score.ToString();
        //icon.transform.parent.GetComponent<Image>().color = isPlayer ? highlightColor : defaultColor;
    }

}
