using UnityEngine;
using UnityEngine.UI;

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

    private CharacterController controller;
    private float stepTimer = 0f;

    private float currentSprint;
    private float cooldownTimer = 0f;
    private bool isSprinting = false;
    private bool shiftHeldLastFrame = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSprint = maxSprint;

        if (sprintSlider != null)
        {
            sprintSlider.maxValue = maxSprint;
            sprintSlider.value = currentSprint;
        }
    }

    void Update()
    {
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

        controller.Move(move * speed * Time.deltaTime);

        // Voetstap geluid
        bool isWalking = move.magnitude > 0.1f;
        float interval = isSprinting ? stepInterval / 2f : stepInterval;

        if (isWalking)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer >= interval)
            {
                footstepSource.PlayOneShot(footstepClip);
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    // ================= SPRINT =================
    private void HandleSprint()
    {
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Als sprint op is of Shift loslaat, start cooldown
        if ((shiftHeld && currentSprint <= 0f) || (!shiftHeld && shiftHeldLastFrame))
        {
            cooldownTimer = sprintCooldown;
        }

        shiftHeldLastFrame = shiftHeld;

        // Sprinten
        if (shiftHeld && currentSprint > 0f)
        {
            isSprinting = true;
            currentSprint -= sprintDepletionRate * Time.deltaTime;
            if (currentSprint < 0f) currentSprint = 0f;
        }
        else
        {
            isSprinting = false;

            // Cooldown actief?
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer < 0f) cooldownTimer = 0f;
            }
            else
            {
                // Regen sprint
                if (currentSprint < maxSprint)
                {
                    currentSprint += sprintRegenRate * Time.deltaTime;
                    if (currentSprint > maxSprint) currentSprint = maxSprint;
                }
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
}
