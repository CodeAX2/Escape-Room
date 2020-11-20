using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceController : MonoBehaviour, Interactable {

    public Camera interactCamera;

    public AudioClip maskRequiredSound, maskGoodSound;

    private Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    void Update() {

    }

    public void OnInteract(PlayerController interactor) {

        interactor.UseInteractCamera(interactCamera, gameObject);
        if (!interactor.InventoryContains("Mask - RM1")) {
            AudioSource.PlayClipAtPoint(maskRequiredSound, transform.position);
            animator.SetTrigger("DenyTrigger");
        } else {
            AudioSource.PlayClipAtPoint(maskGoodSound, transform.position);
            animator.SetTrigger("CheerTrigger");
            GameController.GetInstance().StopTimer();
            Invoke("ClearLevel", maskGoodSound.length);
        }

    }


    private void ClearLevel() {
        GameController.GetInstance().ClearLevel();
    }

    public string GetInteractText() {
        return "Talk to Officer";
    }


}
