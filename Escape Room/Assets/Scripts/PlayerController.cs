using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update

    public Camera playerCamera;
    public float cameraPitchClamp = 80.0f;
    public float mouseSensitivity = 1.0f;

    public float movementSpeed = 2.0f;
    public float sprintSpeed = 3.0f;


    private Rigidbody rb;


    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        UpdateCamera();
        MovePlayer();

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

}