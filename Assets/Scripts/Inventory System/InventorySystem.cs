using System.Collections.Generic;

public class InventorySystem : MonoSingleton<InventorySystem> {

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onItemChangedCallback;

    public int itemSlots = 18;
    public List<ItemStack> items = new List<ItemStack>();

    public ItemData defaultItem;

    public void Start() {

    }

    public bool ChangeInventory(ItemData itemData, int changeQTY) {

        bool isSuccessful = false;

        // Loop through inventory to find if the item already exists.
        foreach(ItemStack inventoryItem in items) {

            if(inventoryItem.itemData == itemData) {

                // see if qty change is legal, if it is legal make the change
                int newQTY = inventoryItem.qty + changeQTY;
                if(newQTY >= 0 && newQTY <= itemData.itemStackSize) {
                    // if there is room in the stack for the entire load
                    inventoryItem.qty = newQTY;
                    if(onItemChangedCallback != null) onItemChangedCallback.Invoke();
                    isSuccessful = true;
                    break;
                } else {
                    // If the stack can only accept part of the load
                    int amtToAdd = itemData.itemStackSize - inventoryItem.qty;
                    changeQTY -= amtToAdd;
                    inventoryItem.qty = itemData.itemStackSize;

                }

            }

        }

        // If the item wasn't already added to a stack and there is room in the inventory for another stack, create a new stack
        if(isSuccessful == false && items.Count < itemSlots) {
            items.Add(new ItemStack(itemData, changeQTY));
            if(onItemChangedCallback != null) onItemChangedCallback.Invoke();
            isSuccessful = true;
        }

        return isSuccessful;

    }


    public bool hasItem(ItemData itemData, int qty = 1) {

        // Loop through inventory to find if the item already exists.
        foreach(ItemStack inventoryItem in items) {
            if(inventoryItem.itemData == itemData && inventoryItem.qty >= qty) return true;
        }

        return false;

    }


}