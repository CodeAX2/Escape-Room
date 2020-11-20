using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAble : MonoBehaviour, Interactable {

    public string itemName;
    public string displayName;

    public AudioClip pickupSound;
    private static List<PickUpAble> allItems;

    public static void RemoveItemByName(string itemName) {

        for (int i = allItems.Count - 1; i >= 0; i--) {
            if (allItems[i].itemName == itemName) {
                PickUpAble item = allItems[i];
                allItems.RemoveAt(i);
                Destroy(item.gameObject);
            }
        }


    }

    private void Awake() {
        if (allItems == null) allItems = new List<PickUpAble>();
        for (int i = allItems.Count - 1; i >= 0; i--) {
            if (allItems[i] == null) allItems.RemoveAt(i);
        }
        allItems.Add(this);
    }

    private void Start() {
        GameObject effect = Instantiate(GameController.GetInstance().pickUpParticleEffect);
        effect.transform.position += transform.position;
        effect.transform.parent = transform;
    }

    public void OnInteract(PlayerController interactor) {
        interactor.AddItemToInventory(itemName);
        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        RemoveItemByName(itemName);
    }

    public string GetInteractText() {
        return " Pick Up " + displayName;
    }


    public string GetItemName() {
        return itemName;
    }
}
