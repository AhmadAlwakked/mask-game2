using UnityEngine;

public class VaultDoor : MonoBehaviour
{
    [Header("Visual")]
    public Outline outline;            // Quick Outline
    public float rotationSpeed = 2f;   // Hoe snel de deur draait

    [Header("Audio")]
    public AudioSource audioSource;    // AudioSource op de deur
    public AudioClip doorSound;        // Geluid bij open/dicht

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        if (outline != null)
            outline.OutlineWidth = 0f;

        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, -90f, 0f);


        // Veiligheid
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
            Debug.LogError("❌ AudioSource ontbreekt op VaultDoor!");
        if (doorSound == null)
            Debug.LogError("❌ DoorSound niet ingesteld!");
    }

    private void Update()
    {
        // Smooth rotatie
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    public void Highlight(bool value)
    {
        if (outline != null)
            outline.OutlineWidth = value ? 5f : 0f;
    }

    public void Toggle()
    {
        // Wissel de staat
        isOpen = !isOpen;

        // Speel geluid direct bij toggle
        if (audioSource != null && doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }

        Debug.Log(isOpen ? "Deur geopend" : "Deur gesloten");
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
