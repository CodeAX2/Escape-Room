using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update

    public Camera playerCamera;
    public float cameraPitchClamp = 80.0f;
    public float mouseSensitivity = 1.0f;

    public float movementSpeed = 2.0f;
    public float sprintSpeed = 3.0f;


    public TextMeshProUGUI pickupText;
    private PickUpAble lookingAt;


    private List<string> inventory;

    private Rigidbody rb;


    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        inventory = new List<string>();

        rb = GetComponent<Rigidbody>();

        pickupText.enabled = false;
    }

    private void Update() {
        UpdateCamera();
        MovePlayer();
        HandlePickups();
        HandleInteract();
    }

    private void UpdateCamera() {
        float mouseY = Input.GetAxis("Mouse Y");
        playerCamera.transform.Rotate(new Vector3(-mouseY * mouseSensitivity, 0, 0));

        if (playerCamera.transform.eulerAngles.x > cameraPitchClamp && playerCamera.transform.eulerAngles.x <= 180) {
            float xRotate = cameraPitchClamp - playerCamera.transform.eulerAngles.x;
            playerCamera.transform.Rotate(new Vector3(xRotate, 0, 0));
        }
        if (playerCamera.transform.eulerAngles.x < (360 - cameraPitchClamp) && playerCamera.transform.eulerAngles.x >= 180) {
            float xRotate = (360 - cameraPitchClamp) - playerCamera.transform.eulerAngles.x;
            playerCamera.transform.Rotate(new Vector3(xRotate, 0, 0));
        }

        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(new Vector3(0, mouseX * mouseSensitivity, 0));
    }

    private void MovePlayer() {
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

    }

    private void HandlePickups() {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            lookingAt = hit.collider.gameObject.GetComponent<PickUpAble>();
            if (lookingAt == null) {
                pickupText.enabled = false;
            } else {


                pickupText.SetText("[E] - Pick Up " + lookingAt.GetItemName());
                pickupText.enabled = true;
            }
        }

        if (Input.GetAxisRaw("PickUp") >= 0.5 && lookingAt != null) {
            lookingAt.PickUp(this);
        }

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


    private bool interactWasDown = false;


    private void HandleInteract() {
        Interactable interactingWith = null;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            interactingWith = hit.collider.gameObject.GetComponent<Interactable>();
            if (interactingWith == null) {
                pickupText.enabled = false;
            } else {


                pickupText.SetText("[E] - Interact ");
                pickupText.enabled = true;
            }
        }

        if (Input.GetAxisRaw("PickUp") >= 0.5 && interactingWith != null && !interactWasDown) {
            interactingWith.OnInteract();
            interactWasDown = true;
        } else if (Input.GetAxisRaw("PickUp") < 0.5) {
            interactWasDown = false;
        }

    }


}