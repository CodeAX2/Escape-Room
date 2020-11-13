using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class KeypadController : MonoBehaviour, Interactable {

    private GameObject digitizer, keyContainer;
    private TextMeshPro digitizerText;
    private Camera keypadCamera;

    public int code;

    public GameObject connectedDoor;


    void Start() {
        SetupKeypad();
    }

    private void SetupKeypad() {
        // Get the digitizer and key container
        for (int i = 0; i < transform.childCount; i++) {
            GameObject curChild = transform.GetChild(i).gameObject;
            if (curChild.CompareTag("Digitizer")) {
                digitizer = curChild;
            } else if (curChild.CompareTag("KeyContainer")) {
                keyContainer = curChild;
            } else if (curChild.CompareTag("KeypadCamera")) {
                keypadCamera = curChild.GetComponent<Camera>();
                keypadCamera.enabled = false;
            }
        }

        digitizerText = digitizer.transform.GetChild(0).GetComponent<TextMeshPro>();
        digitizerText.text = "";


        // Setup each of the keys
        for (int i = 0; i < keyContainer.transform.childCount; i++) {
            GameObject curKey = keyContainer.transform.GetChild(i).gameObject;
            curKey.AddComponent<KeyController>();
            if (i < 9) {
                curKey.GetComponent<KeyController>().SetupKey(this, (KeyController.KeyFunction)(i + 1));
            } else if (i == 9) {
                curKey.GetComponent<KeyController>().SetupKey(this, KeyController.KeyFunction.CLEAR);
            } else if (i == 10) {
                curKey.GetComponent<KeyController>().SetupKey(this, KeyController.KeyFunction.ENTER);
            }


        }

        // Create the code
        System.Random r = new System.Random();
        for (int i = 0; i < 4; i++) {
            code += (int)Math.Pow(10, i) * r.Next(1, 10);
        }


    }

    // Update is called once per frame
    void Update() {

    }


    public void EnterCode() {

        if (Int32.Parse(digitizerText.text) == code) {

            connectedDoor.SetActive(false);

        } else {
            ClearCode();
        }


    }

    public void ClearCode() {
        digitizerText.text = "";
    }

    public void AddNumber(int number) {

        if (digitizerText.text.Length < 4) {
            digitizerText.text += number.ToString();
        }

    }

    public void OnInteract(PlayerController interactor) {
        interactor.UseInteractCamera(keypadCamera, gameObject);
    }

    public string GetInteractText() {
        return "View Keypad";
    }

}
