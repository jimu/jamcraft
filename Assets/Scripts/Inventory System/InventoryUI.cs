using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

    [Header("Item Slot")]
    [SerializeField] private GameObject itemSlotObject;
    [SerializeField] private Transform itemSlotParentTransform;

    InventorySlot[] inventorySlots;

    // Start is called before the first frame update
    void Start() {

        inventorySlots = new InventorySlot[InventorySystem.Instance.itemSlots];

        for(int i = 0; i < InventorySystem.Instance.itemSlots; i++) {
            inventorySlots[i] = Instantiate(itemSlotObject, itemSlotParentTransform).GetComponent<InventorySlot>();
        }

        UpdateInventoryDisplay();
        InventorySystem.Instance.onItemChangedCallback += UpdateInventoryDisplay;
    }

    // Update is called once per frame
    void UpdateInventoryDisplay() {

        ItemStack[] invItems = InventorySystem.Instance.items.ToArray();

        for(int i = 0; i < inventorySlots.Length; i++) {
            
            if(i >= invItems.Length) {
                inventorySlots[i].ClearItem();
            } else {
                inventorySlots[i].SetItem(invItems[i].itemData, invItems[i].qty);
            }


        }

    }
}
