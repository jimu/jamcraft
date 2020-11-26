using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public CraftableData Data
    {
        get { return data; }
        set
        {
            data = value;
            GetComponent<Image>().sprite = data.GetSmallestIcon(defaultSprite);
        }
    }
    
    // usually TBD image (to be done)
    static Sprite defaultSprite = null;

    [SerializeField] private CraftableData data;

    static public void SetDefaultSprite(Sprite sprite)
    {
        defaultSprite = sprite;
    }


}
