using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Transform startParent;
    Vector3 startPos;
    Transform canvas;
    CanvasGroup canvasGroup;

    public void Awake()
    {
        canvas = GameObject.Find("Canvas").transform;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        startParent = transform.parent;
        startPos = transform.localPosition;
        transform.SetParent(canvas, true);
        canvasGroup = GetComponent<CanvasGroup>();

        Crafter.Instance?.RefreshOutput();

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if (transform.parent == canvas)
        {
            transform.SetParent(startParent); // snap back to original owner
            transform.localPosition = startPos;
        }
    }
}
