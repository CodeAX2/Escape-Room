using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class KeypadController : MonoBehaviour {

    GameObject digitizer, keyContainer;

    TextMeshPro digitizerText;

    public int code;


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
            }
        }

        digitizerText = digitizer.transform.GetChild(0).GetComponent<TextMeshPro>();
        digitizerText.text = "";


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

    }

    // Update is called once per frame
    void Update() {

    }


    public void EnterCode() {

        if (Int32.Parse(digitizerText.text) == code) {

            Debug.Log("Correct code!");

        } else {
            Debug.Log("Incorrect code!");
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

}
