using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tets : MonoBehaviour {

    public ItemData it;
    public void test() {
        InventorySystem.Instance.AddToInventory(it, 20);
    }
    public void testrem() {
        InventorySystem.Instance.RemoveFromInventory(it, 20);
    }

}
