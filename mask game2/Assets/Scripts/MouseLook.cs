using UnityEngine;
using TMPro;

public class FirstPersonCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    [Header("Flashlight")]
    public GameObject flashlightObject; // Het parent object van de Light
    private Light flashlightLight;       // Het Light component zelf

    [Header("Battery Settings")]
    [Range(0, 100)]
    public float battery = 100f;
    public float drainInterval = 1f;
    private float drainTimer = 0f;

    [Header("UI")]
    public TMP_Text batteryText;

    private float xRotation = 0f;
    private bool flashlightOn = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (flashlightObject != null)
            flashlightLight = flashlightObject.GetComponentInChildren<Light>();

        if (flashlightLight != null)
            flashlightLight.enabled = flashlightOn;

        UpdateBatteryUI();
    }

    void Update()
    {
        HandleMouseLook();
        HandleFlashlightToggle();
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

    void HandleFlashlightToggle()
    {
        if (Input.GetKeyDown(KeyCode.Q) && flashlightLight != null)
        {
            if (battery > 0f)
            {
                flashlightOn = !flashlightOn;
                flashlightLight.enabled = flashlightOn;
            }
        }

        // Battery leeg → schakel licht uit
        if (battery <= 0f && flashlightOn)
        {
            flashlightOn = false;
            if (flashlightLight != null)
                flashlightLight.enabled = false;
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
            batteryText.text = "Battery: " + Mathf.CeilToInt(battery) + "%";
    }
}
