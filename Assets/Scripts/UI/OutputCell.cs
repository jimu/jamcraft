using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#pragma warning disable 649,414
public class OutputCell : MonoBehaviour
{
    public CraftableData data;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;

    public void Set(CraftableData data)
    {
        //Debug.Log($"{data.name}");

        this.data = data;
        image.sprite = data.GetLargestIcon();
        text.text = data.description;
        SetActive();
    }

    public void SetActive(bool isActive = true)
    {
        gameObject.SetActive(isActive);
    }

    public void OnClick()
    {
        Debug.Log($"Output clicked: {name} - {data.name}");
        Crafter.Instance.OutputClicked(data);
    }
}
