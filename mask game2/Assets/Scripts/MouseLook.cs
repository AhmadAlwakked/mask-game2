using UnityEngine;
using TMPro; // Belangrijk voor TextMeshPro

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    [Header("Flashlight")]
    public GameObject flashlight;

    [Header("Battery Settings")]
    [Range(0, 100)]
    public float battery = 100f;
    public float drainInterval = 1f;
    private float drainTimer = 0f;

    [Header("UI")]
    public TMP_Text batteryText; // Sleep hier je TextMeshPro in

    private float xRotation = 0f;
    private bool flashlightOn = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (flashlight != null)
            flashlight.SetActive(flashlightOn);

        UpdateBatteryUI();
    }

    void Update()
    {
        HandleMouseLook();
        HandleFlashlight();
        DrainBattery();
        UpdateBatteryUI();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void HandleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.Q) && flashlight != null)
        {
            if (battery > 0f)
            {
                flashlightOn = !flashlightOn;
                flashlight.SetActive(flashlightOn);
            }
        }

        if (battery <= 0f && flashlightOn)
        {
            flashlightOn = false;
            flashlight.SetActive(false);
        }
    }

    void DrainBattery()
    {
        if (flashlightOn && battery > 0f)
        {
            drainTimer += Time.deltaTime;
            if (drainTimer >= drainInterval)
            {
                battery -= 1f;
                drainTimer = 0f;
                battery = Mathf.Max(battery, 0f);
            }
        }
    }

    void UpdateBatteryUI()
    {
        if (batteryText != null)
        {
            batteryText.text = "Battery: " + Mathf.CeilToInt(battery) + "%";
        }
    }
}
