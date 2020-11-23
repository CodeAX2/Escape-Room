using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using TMPro;
public class briefcaseController : MonoBehaviour, Interactable
{
    // Start is called before the first frame update
    private Animator anim;
    private GameObject digitizer, keyContainer;
    private TextMeshPro[] digitizerText = new TextMeshPro[4];
    private Camera keypadCamera;

    private String alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private int currentChar = 0;

    public string code= "AAAA";

    public AudioClip keySound, acceptSound;
    private bool isOpen = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        setupBriefcase();
    }

    private void setupBriefcase() {
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
        for (int i = 0; i<digitizer.transform.childCount; i++){
            digitizerText[i] = digitizer.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshPro>();
            digitizerText[i].text = "B";
        }
        for (int i = 0; i < keyContainer.transform.childCount; i++) {
            GameObject curKey = keyContainer.transform.GetChild(i).gameObject;
            curKey.AddComponent<briefcaseKeyController>();
            if (i == 0) {
                curKey.GetComponent<briefcaseKeyController>().SetupKey(this, briefcaseKeyController.KeyFunction.up);
            }else if(i == 1){
                curKey.GetComponent<briefcaseKeyController>().SetupKey(this, briefcaseKeyController.KeyFunction.down);
            }else if(i == 2){
                curKey.GetComponent<briefcaseKeyController>().SetupKey(this, briefcaseKeyController.KeyFunction.right);
            }else if(i == 3){
                curKey.GetComponent<briefcaseKeyController>().SetupKey(this, briefcaseKeyController.KeyFunction.left);
            }
        }
        if (code == "AAAA") {
            // Create the code
            code = "";
            System.Random r = new System.Random();
            for (int i = 0; i < 4; i++) {
                code += alphabet[r.Next(26)];
            }
        }

        GameController.GetInstance().officerText.text = code;

    }

    public void OnInteract(PlayerController interactor) {
        interactor.UseInteractCamera(keypadCamera, gameObject);
    }

    public string GetInteractText() {
        return "View Keypad";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void upCode(){
        int nextLetter = alphabet.IndexOf(digitizerText[currentChar].text);
        if(nextLetter == 25){
            nextLetter = 0;
        }else{
            nextLetter +=1;
        }
        digitizerText[currentChar].text = alphabet[nextLetter].ToString();
        checkCode();
    }

    public void downCode(){
        int nextLetter = alphabet.IndexOf(digitizerText[currentChar].text);
        if(nextLetter == 0){
            nextLetter = 25;
        }else{
            nextLetter -=1;
        }
        digitizerText[currentChar].text = alphabet[nextLetter].ToString();
        checkCode();
    }
    
    public void rightCode(){
        if(currentChar != 3){
            currentChar += 1;
        }
    }

    public void leftCode(){
        if(currentChar != 0){
            currentChar -= 1;
        }
    }

    private void checkCode(){
        int correctCode = 0;
        for(int i = 0; i< 4; i++){
            if(digitizerText[i].text == code[i].ToString()){
                correctCode += 1;
            }
        }
        if(correctCode == 4){
            anim.SetTrigger("open");
            isOpen = true;
        }
    }

}
