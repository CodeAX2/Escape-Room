using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceController : MonoBehaviour, Interactable {

    public Camera interactCamera;

    public AudioClip maskRequiredSound, maskGoodSound;

    void Start() {

    }

    void Update() {

    }

    public void OnInteract(PlayerController interactor) {

        interactor.UseInteractCamera(interactCamera, gameObject);
        if (!interactor.InventoryContains("Mask - RM1")) {
            AudioSource.PlayClipAtPoint(maskRequiredSound, transform.position);
        } else {
            AudioSource.PlayClipAtPoint(maskGoodSound, transform.position);
        }

    }
    public string GetInteractText() {
        return "Talk to Officer";
    }


}
