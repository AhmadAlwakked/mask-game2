using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    [Header("Flashlight")]
    public GameObject flashlight; // Sleep hier je flashlight object in

    private float xRotation = 0f;
    private bool flashlightOn = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (flashlight != null)
            flashlight.SetActive(flashlightOn);
    }

    void Update()
    {
        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        // Toggle flashlight
        if (Input.GetKeyDown(KeyCode.Q) && flashlight != null)
        {
            flashlightOn = !flashlightOn;
            flashlight.SetActive(flashlightOn);
        }
    }
}
