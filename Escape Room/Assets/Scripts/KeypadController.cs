using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using TMPro;
public class KeypadController : Saveable, Interactable {

    private GameObject digitizer, keyContainer;
    private TextMeshPro digitizerText;
    private Camera keypadCamera;

    public int code = 0;

    public GameObject connectedDoor;

    private bool isOpen = false;

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

        if (code == 0) {
            // Create the code
            System.Random r = new System.Random();
            for (int i = 0; i < 4; i++) {
                code += (int)Math.Pow(10, i) * r.Next(1, 10);
            }
        }

    }

    // Update is called once per frame
    void Update() {
    }


    public void EnterCode() {

        if (Int32.Parse(digitizerText.text) == code) {

            isOpen = true;
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

    public override void SaveToFile() {

        Saver s = new Saver();
        s.code = code;
        s.isOpen = isOpen;
        string json = JsonUtility.ToJson(s);


        FileStream file = new FileStream(fileName, FileMode.Truncate);

        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        file.Write(jsonBytes, 0, jsonBytes.Length);

        file.Close();

    }

    public override void LoadFromFile() {

        FileStream file = new FileStream(fileName, FileMode.OpenOrCreate);

        byte[] jsonBytes = new byte[file.Length];
        if (file.Length == 0) {
            file.Close();
            return;
        }
        file.Read(jsonBytes, 0, (int)file.Length);

        string json = Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
        Saver s = JsonUtility.FromJson<Saver>(json);

        code = s.code;
        isOpen = s.isOpen;

        if (isOpen) connectedDoor.SetActive(false);

        file.Close();


    }

    [Serializable]
    private class Saver {

        public int code;
        public bool isOpen;

    }


}
