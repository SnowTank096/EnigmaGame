using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform PlayerBody;
    public float MouseSensitivity = 50f;

    private float _xRotation = 0f;

    private void Start()
    {

        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * MouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * MouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        PlayerBody.Rotate(Vector3.up * mouseX);
    }
}
