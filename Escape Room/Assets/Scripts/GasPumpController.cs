using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasPumpController : MonoBehaviour, Interactable {

    public AudioClip deniedSound;

    public void OnInteract(PlayerController interactor) {
        if (interactor.InventoryContains("GasTankEmpty - RM2") && interactor.InventoryContains("Money - RM2")) {
            interactor.RemoveItemFromInventory("GasTankEmpty - RM2");
            interactor.RemoveItemFromInventory("Money - RM2");
            interactor.AddItemToInventory("GasTankFull - RM2");
            GameController.GetInstance().StopTimer();
            GameController.GetInstance().EnableWinText();
            Invoke("ClearLevel", 2);
        } else {
            AudioSource.PlayClipAtPoint(deniedSound, transform.position);
        }
    }

    public string GetInteractText() {
        return "Use Gas Pump";
    }

    private void ClearLevel() {
        GameController.GetInstance().ClearLevel();
    }

}
