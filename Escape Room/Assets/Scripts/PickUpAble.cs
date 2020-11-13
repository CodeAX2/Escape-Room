using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAble : MonoBehaviour, Interactable {

    public string itemName;

    private void Start() {
        GameObject effect = Instantiate(GameController.GetInstance().pickUpParticleEffect);
        effect.transform.position += transform.position;
        effect.transform.parent = transform;
    }

    public void OnInteract(PlayerController interactor){
        interactor.AddItemToInventory(itemName);
        Destroy(gameObject);
    }

    public string GetInteractText(){
        return " Pick Up " + itemName;
    }


    public string GetItemName() {
        return itemName;
    }

}
