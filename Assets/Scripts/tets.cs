using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tets : MonoBehaviour {

    public ItemData it;
    public void test() {
        InventorySystem.Instance.ChangeInventory(it, 20);
    }

}
