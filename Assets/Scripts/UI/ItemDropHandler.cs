using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        eventData.pointerDrag.transform.SetParent(transform);
        eventData.pointerDrag.transform.localPosition = Vector3.zero;

        Crafter.Instance?.RefreshOutput();
    }

}
