using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f; // Snelheid van de muis
    public Transform playerBody;           // Player object waar de camera een child van is

    private float xRotation = 0f;          // Voor verticale rotatie

    void Start()
    {
        // Cursor verbergen en vastzetten in het midden van het scherm
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Muis input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Verticaal kijken (op en neer)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Beperk naar boven/onder

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontaal draaien (links/rechts) van de player
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
