using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header ("Sensitivity")]
    public float senX;
    public float senY;
    public float maxSenAmount;
    public Transform orientation;
    public float camTiltAngle = 5f;
    public float camTiltSpeed = 2f;
    float xRotation;
    float yRotation;
    public bool canControl;

    [Header("Cam Shake")]
    [SerializeField] AnimationCurve shakeCurve;
    [SerializeField] float duration = 1f;
    [SerializeField] float intensity = 1f;
    private void Start()
    {
        UnlockCam();
    }
    void Update()
    {
        if (!canControl) return;
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        HandleCameraTilt();
    }

    public void MouseSen(float value)
    {
        float localValue = value * maxSenAmount;

        senX = localValue;
        senY = localValue;
    }
    public void LockCam()
    {
        canControl = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void UnlockCam()
    {
        canControl = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void HandleCameraTilt()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float targetAngle = -horizontalInput * camTiltAngle;

        Quaternion targetRotation = Quaternion.Euler(0, transform.localEulerAngles.y, targetAngle);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * camTiltSpeed);
    }
    public void ShakeCam()
    {
        StartCoroutine(Shake());
    }
    IEnumerator Shake()
    {
        Vector2 startPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float _intensity = shakeCurve.Evaluate(elapsedTime / duration) * intensity;
            transform.localPosition = startPos + Random.insideUnitCircle * _intensity;
            yield return null;
        }

        transform.localPosition = startPos;
    }
}
