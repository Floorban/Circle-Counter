using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header ("Sensitivity")]
    public float senX;
    public float senY;
    public Transform orientation;
    public float camTiltAngle = 5f;
    public float camTiltSpeed = 2f;
    float xRotation;
    float yRotation;

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        HandleCameraTilt();
    }
    private void HandleCameraTilt()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float targetAngle = -horizontalInput * camTiltAngle;

        Quaternion targetRotation = Quaternion.Euler(0, transform.localEulerAngles.y, targetAngle);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * camTiltSpeed);
    }
}
