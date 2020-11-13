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


    public float interactDistance = 2.0f;
    public TextMeshProUGUI interactText;
    private Interactable lookingAt;
    private bool interactWasDown;
    private Camera interactCamera;
    private Vector3 initialInteractCameraPosition;
    private Quaternion initialInteractCameraRotation;
    private bool interacting = false;
    private GameObject interactingWith = null;


    private List<string> inventory;

    private Rigidbody rb;


    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        inventory = new List<string>();

        rb = GetComponent<Rigidbody>();

        interactText.enabled = false;
    }

    private void Update() {
        UpdateCamera();
        MovePlayer();
        HandleInteract();
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
        this.interactingWith.gameObject.GetComponent<BoxCollider>().enabled = false;
        playerCamera.gameObject.SetActive(false);
        interacting = true;
        this.interactCamera.enabled = true;
    }

    public void ResetInteracting() {
        interactCamera.transform.position = initialInteractCameraPosition;
        interactCamera.transform.rotation = initialInteractCameraRotation;
        this.interactCamera.enabled = false;
        interactCamera.gameObject.SetActive(false);
        interactCamera = null;
        interactingWith.gameObject.GetComponent<BoxCollider>().enabled = true;
        interactingWith = null;
        playerCamera.gameObject.SetActive(true);
        interacting = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public bool IsInteractingWith(GameObject other) {
        return interactingWith.Equals(other);
    }


}