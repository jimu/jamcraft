
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoSingleton<InventorySystem> {

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onItemChangedCallback;

    public int itemSlots = 18;
    public List<ItemStack> items = new List<ItemStack>();

    public ItemData defaultItem;

    public void Start() {

    }

    // Will return the amount of items that were unable to be added to the inventory
    public int AddToInventory(ItemData itemData, int addQTY) {

        bool isSuccessful = false;

        if(addQTY < 0) {
            Debug.LogError("Trying to add a negative amount of items to inventory!");
        }

        // Loop through inventory to find if the item already exists.
        foreach(ItemStack inventoryItem in items) {

            if(inventoryItem.itemData == itemData) {

                // see if qty change is legal, if it is legal make the change
                int newQTY = inventoryItem.qty + addQTY;
                if(newQTY >= 0 && newQTY <= itemData.itemStackSize) {
                    // if there is room in the stack for the entire load
                    inventoryItem.qty = newQTY;
                    isSuccessful = true;
                    break;
                } else {
                    // If the stack can only accept part of the load
                    int amtToAdd = itemData.itemStackSize - inventoryItem.qty;
                    addQTY -= amtToAdd;
                    inventoryItem.qty = itemData.itemStackSize;
                }

            }

        }

        // If the item wasn't already added to a stack and there is room in the inventory for another stack, create a new stack
        if(isSuccessful == false && items.Count < itemSlots) {
            items.Add(new ItemStack(itemData, addQTY));
            isSuccessful = true;
        }

        if(onItemChangedCallback != null) onItemChangedCallback.Invoke();

        return addQTY;

    }

    public void RemoveFromInventory(ItemData itemData, int removeQTY) {

        if(removeQTY < 0) Debug.LogError("Trying to remove a negative amount of items from inventory!");

        for(int i = items.Count - 1; i >= 0; i--) {

            ItemStack inventoryItem = items[i];

            if(inventoryItem.itemData == itemData) {

                // If there is enough in the stack just remove the amount and then return;
                if(removeQTY < inventoryItem.qty) {
                    inventoryItem.qty -= removeQTY;
                    break;
                } else {
                    removeQTY -= inventoryItem.qty;
                    items.RemoveAt(i);
                }

            }

        }

        if(onItemChangedCallback != null) onItemChangedCallback.Invoke();

    }



    public bool hasItem(ItemData itemData, int qty = 1) {

        int qtyFound = 0;

        // Loop through inventory to find if the item already exists.
        foreach(ItemStack inventoryItem in items) {

            if(inventoryItem.itemData == itemData) {
                qtyFound += inventoryItem.qty;
                if(qtyFound >= qty) return true;
            }

        }

        return false;

    }


}