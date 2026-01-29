using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public AudioSource footstepSource;
    public AudioClip footstepClip;
    public float stepInterval = 0.5f;

    // 👈 BOOL DIE BIJHOUDT OF DE SPELER BINNEN EEN GESLOTEN VAULT STAAT
    public bool inside = false;

    private CharacterController controller;
    private float stepTimer = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        bool isWalking = move.magnitude > 0.1f;

        if (isWalking)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
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

    // 🔹 Trigger logic voor Vault collider
    private void OnTriggerEnter(Collider other)
    {
        Vault vault = other.GetComponent<Vault>();
        if (vault != null)
        {
            // Alleen true als speler tegen de vault staat en de deur gesloten is
            if (!vault.door.IsOpen())
            {
                inside = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Vault vault = other.GetComponent<Vault>();
        if (vault != null)
        {
            // Blijft true zolang deur gesloten is, anders false
            inside = !vault.door.IsOpen();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Vault vault = other.GetComponent<Vault>();
        if (vault != null)
        {
            inside = false;
        }
    }
}
