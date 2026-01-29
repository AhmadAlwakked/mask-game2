using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float maxDistance = 5f;        // maximale afstand voor interactie
    public float sphereRadius = 0.2f;     // radius van de spherecast voor dot precision

    [Header("Crosshair UI")]
    public Image crosshair;               // je UI dot/crosshair
    public Color normalColor = Color.red;
    public Color targetColor = Color.white;

    private Camera playerCamera;
    private CashObject lastHitCash;
    private VaultDoor lastHitVaultDoor;

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

        // SphereCast vanaf camera richting het midden van het scherm (dot)
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereRadius, out hit, maxDistance))
        {
            float distance = Vector3.Distance(playerCamera.transform.position, hit.collider.transform.position);

            if (distance <= maxDistance)
            {
                // Cash detectie
                if (hit.collider.CompareTag("Cash"))
                {
                    currentCash = hit.collider.GetComponent<CashObject>();
                    if (currentCash != null)
                        currentCash.Highlight(true);
                }

                // Vault detectie (VaultDoor)
                if (hit.collider.CompareTag("Vault"))
                {
                    currentVaultDoor = hit.collider.GetComponent<VaultDoor>();
                    if (currentVaultDoor != null)
                        currentVaultDoor.Highlight(true);
                }
            }
        }

        // Verwijder highlight als object niet meer bekeken wordt
        if (lastHitCash != null && lastHitCash != currentCash)
            lastHitCash.Highlight(false);

        if (lastHitVaultDoor != null && lastHitVaultDoor != currentVaultDoor)
            lastHitVaultDoor.Highlight(false);

        lastHitCash = currentCash;
        lastHitVaultDoor = currentVaultDoor;

        // Update crosshair kleur
        if (crosshair != null)
            crosshair.color = (currentCash != null || currentVaultDoor != null) ? targetColor : normalColor;

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
                currentVaultDoor.Toggle(); // open of sluit de kluisdeur
            }
        }
    }
}
