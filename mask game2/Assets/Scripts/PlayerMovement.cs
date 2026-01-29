using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public AudioSource footstepSource; // audio source op je player
    public AudioClip footstepClip;     // je voetstapgeluid
    public float stepInterval = 0.5f;  // elke 0.5 seconde

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

        bool isWalking = move.magnitude > 0.1f; // 👈 geen isGrounded meer

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
            stepTimer = 0f; // reset timer als je stopt
        }
    }
}
