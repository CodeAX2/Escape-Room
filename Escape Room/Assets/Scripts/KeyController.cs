using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyController : MonoBehaviour, Interactable {


    public enum KeyFunction {
        UNSET = -1,
        ONE = 1,
        TWO = 2,
        THREE = 3,
        FOUR = 4,
        FIVE = 5,
        SIX = 6,
        SEVEN = 7,
        EIGHT = 8,
        NINE = 9,
        ENTER = 0,
        CLEAR = 10
    };

    KeyFunction function = KeyFunction.UNSET;
    KeypadController controller;

    public void SetupKey(KeypadController controller, KeyFunction function) {
        this.controller = controller;
        this.function = function;
    }

    public void OnInteract(PlayerController interactor) {

        if (function >= KeyFunction.ONE && function <= KeyFunction.NINE) {
            controller.AddNumber((int)function);
        } else if (function == KeyFunction.CLEAR) {
            controller.ClearCode();
        } else if (function == KeyFunction.ENTER) {
            controller.EnterCode();
        }

    }

    public string GetInteractText() {
        return "Press Key";
    }



}
