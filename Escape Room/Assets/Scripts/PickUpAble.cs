using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAble : MonoBehaviour {

    public string itemName;

    public void PickUp(PlayerController picker) {
        picker.AddItemToInventory(itemName);
        Destroy(gameObject);
    }

    public string GetItemName() {
        return itemName;
    }

}
