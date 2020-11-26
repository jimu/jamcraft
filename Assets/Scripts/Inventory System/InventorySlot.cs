
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#pragma warning disable 649

public class InventorySlot : MonoBehaviour {

    ItemData itemData;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemQTY;

    public void SetItem(ItemData item, int qty) {
        itemData = item;
        itemIcon.sprite = item.itemIcon;
        itemIcon.color = new Color(1, 1, 1, 1);
        if(qty >= 0 && qty <= 1) {
            itemQTY.text = "";
        } else {
            itemQTY.text = qty.ToString();
        }
    }

    public void ClearItem() {
        itemData = null;
        itemIcon.sprite = null;
        itemQTY.text = "";
        itemIcon.color = new Color(0, 0, 0, 0);
    }


}
