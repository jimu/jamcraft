using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 649,414
public class FlyoutOutputPanel : MonoBehaviour
{

    [SerializeField] Transform container;
    [SerializeField] GameObject cellPrefab;
    [SerializeField] List<OutputCell> cells;

    void Set(List<CraftableData> list)
    {
        int i;

        for (i = 0; i < list.Count; ++i)
        {
            if (i >= cells.Count)
                cells.Add(Instantiate(cellPrefab, container).GetComponent<OutputCell>());

            cells[i].Set(list[i]);
        }

        for (; i < cells.Count; ++i)
            cells[i].SetActive(false);
    }

    public void Activate(List<CraftableData> list)
    {
        Set(list);
        gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

}
