
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Jamcraft/Item")]
public class ItemData : ScriptableObject {

    [Tooltip("Item Name")]
    public string itemName;

    [Tooltip("Item Icon")]
    public Sprite itemIcon;

    [Tooltip("Item Stack Size")]
    [Min(1)]
    public int itemStackSize;

}
