using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using TMPro;
public class briefcaseController : MonoBehaviour//, Interactable
{
    // Start is called before the first frame update
    private Animator anim;
    private GameObject digitizer, keyContainer;
    private TextMeshPro[] digitizerText = new TextMeshPro[4];
    private Camera keypadCamera;

    public string code= "";

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
            digitizerText[i] = digitizer.transform.GetChild(i).GetComponent<TextMeshPro>();
            digitizerText[i].text = "";
        }
        for (int i = 0; i < keyContainer.transform.childCount; i++) {
            GameObject curKey = keyContainer.transform.GetChild(i).gameObject;
            curKey.AddComponent<briefcaseKeyController>();
            if (i == 0) {
                curKey.GetComponent<briefcaseKeyController>().SetupKey(this, briefcaseKeyController.KeyFunction.up);
            }else if(i == 1){
                curKey.GetComponent<briefcaseKeyController>().SetupKey(this, briefcaseKeyController.KeyFunction.down);
            }else if(i == 1){
                curKey.GetComponent<briefcaseKeyController>().SetupKey(this, briefcaseKeyController.KeyFunction.right);
            }else if(i == 1){
                curKey.GetComponent<briefcaseKeyController>().SetupKey(this, briefcaseKeyController.KeyFunction.left);
            }
        }
        /*if (code == "") {
            // Create the code
            System.Random r = new System.Random();
            for (int i = 0; i < 4; i++) {
                code += (int)Math.Pow(10, i) * r.Next(1, 10);
            }
        }*/

        GameController.GetInstance().officerText.text = code.ToString();

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

    }

    public void downCode(){

    }
    
    public void rightCode(){

    }

    public void leftCode(){

    }

}
