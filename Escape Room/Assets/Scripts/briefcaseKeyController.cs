using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class briefcaseKeyController : MonoBehaviour, Interactable {


    public enum KeyFunction {
        UNSET = -1,
        up = 1,
        down = 2,
        right = 3,
        left = 4,
    };

    KeyFunction function = KeyFunction.UNSET;
    briefcaseController controller;

    public void SetupKey(briefcaseController controller, KeyFunction function) {
        this.controller = controller;
        this.function = function;
    }

    public void OnInteract(PlayerController interactor) {

        AudioSource.PlayClipAtPoint(controller.keySound, transform.position);

        if (function == KeyFunction.up) {
            controller.upCode();
        } else if (function == KeyFunction.down) {
            controller.downCode();
        }else if (function == KeyFunction.right) {
            controller.rightCode();
        }else if (function == KeyFunction.left) {
            controller.leftCode();
        }

    }

    public string GetInteractText() {
        return "Press Key";
    }



}
