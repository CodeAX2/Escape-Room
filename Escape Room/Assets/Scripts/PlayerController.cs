﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System;
using System.Text;

public class PlayerController : Saveable {
    // Start is called before the first frame update

    public Camera playerCamera;
    public float cameraPitchClamp = 80.0f;
    public float mouseSensitivity = 1.0f;
    public float movementSpeed = 2.0f;
    public float sprintSpeed = 3.0f;


    public float interactDistance = 2.0f;
    public TextMeshProUGUI interactText, exitText;
    public GameObject maskImage, keyImage, moneyImage, garbageImage, emptyGasImage, fullGasImage;
    private Interactable lookingAt;
    private bool interactWasDown;
    private Camera interactCamera;
    private Vector3 initialInteractCameraPosition;
    private Quaternion initialInteractCameraRotation;
    private bool interacting = false;
    private GameObject interactingWith = null;
    public Animator playerAnimator;


    private List<string> inventory;

    private Rigidbody rb;


    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        if (inventory == null) inventory = new List<string>();

        rb = GetComponent<Rigidbody>();

        interactText.enabled = false;
        exitText.enabled = false;
    }

    private void Update() {
        UpdateCamera();
        MovePlayer();
        HandleInteract();
        UpdateUI();
    }

    private void UpdateCamera() {
        float mouseY = Input.GetAxis("Mouse Y");
        Camera cameraToUse = playerCamera;

        if (interacting)
            cameraToUse = interactCamera;

        cameraToUse.transform.Rotate(new Vector3(-mouseY * mouseSensitivity, 0, 0));

        if (cameraToUse.transform.eulerAngles.x > cameraPitchClamp && cameraToUse.transform.eulerAngles.x <= 180) {
            float xRotate = cameraPitchClamp - cameraToUse.transform.eulerAngles.x;
            cameraToUse.transform.Rotate(new Vector3(xRotate, 0, 0));
        }
        if (cameraToUse.transform.eulerAngles.x < (360 - cameraPitchClamp) && cameraToUse.transform.eulerAngles.x >= 180) {
            float xRotate = (360 - cameraPitchClamp) - cameraToUse.transform.eulerAngles.x;
            cameraToUse.transform.Rotate(new Vector3(xRotate, 0, 0));
        }

        float mouseX = Input.GetAxis("Mouse X");
        if (!interacting)
            transform.Rotate(new Vector3(0, mouseX * mouseSensitivity, 0));
        else
            cameraToUse.transform.Rotate(new Vector3(0, mouseX * mouseSensitivity, 0), Space.World);
    }

    private void MovePlayer() {

        if (interacting) return;

        float forwardAmount = Input.GetAxisRaw("Vertical");
        float sideAmount = Input.GetAxisRaw("Horizontal");

        float speedToUse = movementSpeed;
        if (Input.GetAxisRaw("Sprint") >= 0.5) {
            speedToUse = sprintSpeed;
        }

        Vector3 newVelocity = transform.forward * forwardAmount + transform.right * sideAmount;
        newVelocity.Normalize();
        newVelocity *= speedToUse;

        newVelocity.y = rb.velocity.y;

        rb.velocity = newVelocity;

        if (rb.velocity.magnitude >= 0.05f) {
            playerAnimator.SetBool("walking", true);
        } else {
            playerAnimator.SetBool("walking", false);
        }

    }

    private void HandleInteract() {

        Camera cameraToUse = playerCamera;

        if (interacting)
            cameraToUse = interactCamera;

        Ray ray = cameraToUse.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.distance < interactDistance) {

                lookingAt = hit.collider.gameObject.GetComponent<Interactable>();

                if (lookingAt == null) {
                    interactText.enabled = false;
                } else {

                    interactText.SetText("[E] - " + lookingAt.GetInteractText());
                    interactText.enabled = true;

                }
            } else {
                interactText.enabled = false;
            }
        }

        if (Input.GetAxisRaw("Interact") >= 0.5 && lookingAt != null && !interactWasDown) {
            lookingAt.OnInteract(this);
            interactWasDown = true;
        } else if (Input.GetAxisRaw("Interact") < 0.5) {
            interactWasDown = false;
        }

        if (Input.GetAxisRaw("StopInteract") >= 0.5 && interacting) {
            ResetInteracting();
        }

        if (interactingWith != null && !interactingWith.activeInHierarchy) {
            ResetInteracting();
        }

    }

    private void UpdateUI() {

        if (maskImage != null)
            maskImage.SetActive(InventoryContains("Mask - RM1"));
        if (keyImage != null)
            keyImage.SetActive(InventoryContains("CarKey - RM1"));

        if (moneyImage != null)
            moneyImage.SetActive(InventoryContains("Money - RM2"));
        if (garbageImage != null)
            garbageImage.SetActive(InventoryContains("Trash - RM2") || InventoryContains("Trash2 - RM2"));
        if (emptyGasImage != null)
            emptyGasImage.SetActive(InventoryContains("GasTankEmpty - RM2"));
        if (fullGasImage != null)
            fullGasImage.SetActive(InventoryContains("GasTankFull - RM2"));


    }

    public void AddItemToInventory(string itemName) {
        inventory.Add(itemName);
    }

    public bool InventoryContains(string itemName) {
        return inventory.Contains(itemName);
    }

    public int AmountInInventory(string itemName) {
        int total = 0;
        foreach (string item in inventory) {

            if (item.Equals(itemName)) total++;

        }
        return total;
    }

    public void RemoveItemFromInventory(string itemName) {
        inventory.Remove(itemName);
    }

    public void UseInteractCamera(Camera interactCamera, GameObject interactingWith) {

        if (interacting) {
            // Clean up previous interacting
            ResetInteracting();
        }

        this.interactCamera = interactCamera;
        initialInteractCameraPosition = interactCamera.transform.position;
        initialInteractCameraRotation = interactCamera.transform.rotation;
        this.interactCamera.gameObject.SetActive(true);
        this.interactingWith = interactingWith;
        this.interactingWith.gameObject.GetComponent<Collider>().enabled = false;
        playerCamera.gameObject.SetActive(false);
        interacting = true;
        this.interactCamera.enabled = true;
        exitText.enabled = true;
    }

    public void ResetInteracting() {
        interactCamera.transform.position = initialInteractCameraPosition;
        interactCamera.transform.rotation = initialInteractCameraRotation;
        this.interactCamera.enabled = false;
        interactCamera.gameObject.SetActive(false);
        interactCamera = null;
        interactingWith.gameObject.GetComponent<Collider>().enabled = true;
        interactingWith = null;
        playerCamera.gameObject.SetActive(true);
        interacting = false;
        Cursor.lockState = CursorLockMode.Locked;
        exitText.enabled = false;
    }

    public bool IsInteractingWith(GameObject other) {
        return interactingWith.Equals(other);
    }

    public override void LoadFromFile() {
        Directory.CreateDirectory(Path.GetDirectoryName(GetFileName()));
        FileStream file = new FileStream(GetFileName(), FileMode.OpenOrCreate);

        byte[] jsonBytes = new byte[file.Length];
        if (file.Length == 0) {
            file.Close();
            return;
        }
        file.Read(jsonBytes, 0, (int)file.Length);

        string json = Encoding.UTF8.GetString(jsonBytes, 0, jsonBytes.Length);
        Saver s = JsonUtility.FromJson<Saver>(json);

        inventory = s.inventory;

        file.Close();

        foreach (string itemName in inventory) {
            PickUpAble.RemoveItemByName(itemName);
        }

    }


    public override void SaveToFile() {

        Saver s = new Saver();
        s.inventory = inventory;
        string json = JsonUtility.ToJson(s);

        Directory.CreateDirectory(Path.GetDirectoryName(GetFileName()));
        FileStream file = new FileStream(GetFileName(), FileMode.Truncate);

        byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
        file.Write(jsonBytes, 0, jsonBytes.Length);

        file.Close();

    }

    [Serializable]
    private class Saver {

        public List<string> inventory;

    }

}