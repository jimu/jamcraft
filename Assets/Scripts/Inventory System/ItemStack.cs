[System.Serializable]
public class ItemStack {

    public ItemData itemData;
    public int qty;

    public ItemStack(ItemData itemData, int qty) {
        this.itemData = itemData;
        this.qty = qty;
    }
}
