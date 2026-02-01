using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float maxDistance = 5f;
    public float sphereRadius = 0.2f;

    [Header("Crosshair UI")]
    public Image crosshair;
    public Color normalColor = Color.red;
    public Color targetColor = Color.white;

    private Camera playerCamera;

    // Houd bij wat we eerder hebben geraakt
    private CashObject lastHitCash;
    private VaultDoor lastHitVaultDoor;
    private PianoObject lastHitPiano;

    void Start()
    {
        playerCamera = Camera.main;
        if (crosshair != null)
            crosshair.color = normalColor;
    }

    void Update()
    {
        CashObject currentCash = null;
        VaultDoor currentVaultDoor = null;
        PianoObject currentPiano = null;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereRadius, maxDistance);

        foreach (RaycastHit hit in hits)
        {
            // Negeer de speler zelf
            if (hit.collider.CompareTag("Player"))
                continue;

            // Cash detectie
            if (hit.collider.CompareTag("Cash"))
            {
                currentCash = hit.collider.GetComponent<CashObject>();
                if (currentCash != null)
                    currentCash.Highlight(true);
            }

            // Vault detectie
            if (hit.collider.CompareTag("Vault"))
            {
                currentVaultDoor = hit.collider.GetComponent<VaultDoor>();
                if (currentVaultDoor != null)
                    currentVaultDoor.Highlight(true);
            }

            // Piano detectie (ook als collider child van piano is)
            if (hit.collider.CompareTag("Piano"))
            {
                currentPiano = hit.collider.GetComponentInParent<PianoObject>();
                if (currentPiano != null)
                    currentPiano.Highlight(true);
            }

            // Stop bij eerste interactable
            if (currentCash != null || currentVaultDoor != null || currentPiano != null)
                break;
        }

        // Verwijder highlight van vorige objecten die we nu niet meer raken
        if (lastHitCash != null && lastHitCash != currentCash)
            lastHitCash.Highlight(false);

        if (lastHitVaultDoor != null && lastHitVaultDoor != currentVaultDoor)
            lastHitVaultDoor.Highlight(false);

        if (lastHitPiano != null && lastHitPiano != currentPiano)
            lastHitPiano.Highlight(false);

        lastHitCash = currentCash;
        lastHitVaultDoor = currentVaultDoor;
        lastHitPiano = currentPiano;

        // Update crosshair kleur
        if (crosshair != null)
            crosshair.color = (currentCash != null || currentVaultDoor != null || currentPiano != null) ? targetColor : normalColor;

        // Interactie met E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentCash != null)
            {
                currentCash.Collect();
                lastHitCash = null;
            }

            if (currentVaultDoor != null)
            {
                currentVaultDoor.Toggle();
            }

            if (currentPiano != null)
            {
                currentPiano.TogglePiano();
            }
        }
    }
}
