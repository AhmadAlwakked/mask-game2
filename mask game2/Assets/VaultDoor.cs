using UnityEngine;

public class VaultDoor : MonoBehaviour
{
    public Outline outline;          // Quick Outline component
    public float rotationSpeed = 2f; // Hoe snel de deur draait

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        if (outline != null)
            outline.OutlineWidth = 0f;

        closedRotation = transform.rotation;
        openRotation = closedRotation * Quaternion.Euler(0f, -90f, 0f); // draai Y -90 graden als open
    }

    private void Update()
    {
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void Highlight(bool value)
    {
        if (outline != null)
            outline.OutlineWidth = value ? 5f : 0f;
    }

    public void Toggle()
    {
        isOpen = !isOpen;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
