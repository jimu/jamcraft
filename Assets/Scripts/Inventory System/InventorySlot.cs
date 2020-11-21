
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour {

    ItemData itemData;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemQTY;

    public void SetItem(ItemData item, int qty) {
        itemData = item;
        itemIcon.sprite = item.itemIcon;
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
    }


}
