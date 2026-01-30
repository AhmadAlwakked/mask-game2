using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    public AudioSource footstepSource;
    public AudioClip footstepClip;
    public float stepInterval = 0.5f;

    [Header("Sprint Settings")]
    public float maxSprint = 5f;
    public float sprintDepletionRate = 1f;
    public float sprintRegenRate = 1f;
    public float sprintCooldown = 3f;
    public Slider sprintSlider;

    public bool inside = false; // Vault logic

    [Header("Gravity Settings")]
    public float gravity = -9.81f;
    private float verticalVelocity = 0f;

    [Header("Game Over UI")]
    public GameObject gameOverPanel; // Extra panel naast loseScreen

    private CharacterController controller;
    private float stepTimer = 0f;

    private float currentSprint;
    private float cooldownTimer = 0f;
    private bool isSprinting = false;
    private bool shiftHeldLastFrame = false;

    private bool isDead = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSprint = maxSprint;

        if (sprintSlider != null)
        {
            sprintSlider.maxValue = maxSprint;
            sprintSlider.value = currentSprint;
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Cursor verbergen en vergrendelen bij start
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleSprint();
        UpdateSlider();
    }

    // ================= MOVEMENT =================
    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        float speed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        // Zwaartekracht
        if (controller.isGrounded)
            verticalVelocity = -1f;
        else
            verticalVelocity += gravity * Time.deltaTime;

        move.y = verticalVelocity;
        controller.Move(move * speed * Time.deltaTime);

        // Voetstap geluid
        bool isWalking = new Vector3(moveX, 0, moveZ).magnitude > 0.1f;
        float interval = isSprinting ? stepInterval / 2f : stepInterval;

        if (isWalking && controller.isGrounded)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= interval)
            {
                footstepSource?.PlayOneShot(footstepClip);
                stepTimer = 0f;
            }
        }
        else stepTimer = 0f;
    }

    // ================= SPRINT =================
    private void HandleSprint()
    {
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if ((shiftHeld && currentSprint <= 0f) || (!shiftHeld && shiftHeldLastFrame))
            cooldownTimer = sprintCooldown;

        shiftHeldLastFrame = shiftHeld;

        if (shiftHeld && currentSprint > 0f)
        {
            isSprinting = true;
            currentSprint -= sprintDepletionRate * Time.deltaTime;
            currentSprint = Mathf.Max(currentSprint, 0f);
        }
        else
        {
            isSprinting = false;

            if (cooldownTimer > 0f)
                cooldownTimer -= Time.deltaTime;
            else if (currentSprint < maxSprint)
            {
                currentSprint += sprintRegenRate * Time.deltaTime;
                currentSprint = Mathf.Min(currentSprint, maxSprint);
            }
        }
    }

    // ================= UI =================
    private void UpdateSlider()
    {
        if (sprintSlider != null)
            sprintSlider.value = currentSprint;
    }

    // ================= VAULT =================
    private void OnTriggerEnter(Collider other)
    {
        Vault vault = other.GetComponent<Vault>();
        if (vault != null && !vault.door.IsOpen())
            inside = true;

        // ================= GAME OVER =================
        if (!isDead && other.CompareTag("Monster"))
        {
            Die();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Vault vault = other.GetComponent<Vault>();
        if (vault != null)
            inside = !vault.door.IsOpen();
    }

    private void OnTriggerExit(Collider other)
    {
        Vault vault = other.GetComponent<Vault>();
        if (vault != null)
            inside = false;
    }

    // ================= GAME OVER LOGICA =================
    private void Die()
    {
        isDead = true;
        Debug.Log("Game Over! Je bent gepakt door een monster!");

        // Stop speler volledig
        enabled = false;

        // Cursor terugzetten
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Verberg maskers en panel UI
        MaskScalerUI maskUI = FindObjectOfType<MaskScalerUI>();
        if (maskUI != null)
        {
            maskUI.mask1.gameObject.SetActive(false);
            maskUI.mask2.gameObject.SetActive(false);
            maskUI.mask3.gameObject.SetActive(false);
            maskUI.mask1Object?.SetActive(false);
            maskUI.mask2Object?.SetActive(false);
            maskUI.mask3Object?.SetActive(false);
            if (maskUI.panel != null) maskUI.panel.gameObject.SetActive(false);

            // Zet lose screen aan
            if (maskUI.loseScreen != null)
                maskUI.loseScreen.SetActive(true);
        }

        // GameOver panel aan
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}
