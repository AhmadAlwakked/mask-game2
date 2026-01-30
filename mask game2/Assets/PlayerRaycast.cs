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

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereRadius, maxDistance);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
                continue;

            if (hit.collider.CompareTag("Cash"))
            {
                currentCash = hit.collider.GetComponent<CashObject>();
                if (currentCash != null)
                    currentCash.Highlight(true);
            }

            if (hit.collider.CompareTag("Vault"))
            {
                currentVaultDoor = hit.collider.GetComponent<VaultDoor>();
                if (currentVaultDoor != null)
                    currentVaultDoor.Highlight(true);
            }

            if (currentCash != null || currentVaultDoor != null)
                break;
        }

        if (lastHitCash != null && lastHitCash != currentCash)
            lastHitCash.Highlight(false);

        if (lastHitVaultDoor != null && lastHitVaultDoor != currentVaultDoor)
            lastHitVaultDoor.Highlight(false);

        lastHitCash = currentCash;
        lastHitVaultDoor = currentVaultDoor;

        if (crosshair != null)
            crosshair.color = (currentCash != null || currentVaultDoor != null) ? targetColor : normalColor;

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
        }
    }
}
