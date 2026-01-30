using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // voor scene reload als alternatief

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
    public float gravity = -9.81f; // negatieve waarde voor naar beneden trekken
    private float verticalVelocity = 0f;

    [Header("Game Over UI")]
    public GameObject gameOverPanel; // sleep hier een UI panel voor Game Over

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

        // Zorg dat Game Over panel uit staat bij start
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
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

        // ================= ZWAARTEKRACHT =================
        if (controller.isGrounded)
        {
            verticalVelocity = -1f; // licht naar beneden trekken zodat grounded blijft
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime; // zwaartekracht toepassen
        }

        // Voeg verticale beweging toe
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

    // ================= GAME OVER =================
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Monster"))
        {
            Debug.Log("Game Over! Je bent gepakt door een monster!");

            // Toon Game Over UI panel
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);

            // Stop beweging
            enabled = false;

            // Optioneel: herlaad de scene na 3 seconden
            // StartCoroutine(ReloadSceneAfterDelay(3f));
        }
    }

    // Optionele coroutine om scene te herladen
    private System.Collections.IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
