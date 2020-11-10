using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAble : MonoBehaviour {

    public string itemName;

    private void Start() {
        GameObject effect = Instantiate(GameController.GetInstance().pickUpParticleEffect);
        effect.transform.position += transform.position;
        effect.transform.parent = transform;
    }

    public void PickUp(PlayerController picker) {
        picker.AddItemToInventory(itemName);
        Destroy(gameObject);
    }

    public string GetItemName() {
        return itemName;
    }

}
