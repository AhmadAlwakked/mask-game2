using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float maxDistance = 5f;        // Maximale afstand om interactie te hebben
    public string targetTag = "Cash";     // Tag van cash objecten

    [Header("Crosshair UI")]
    public Image crosshair;
    public Color normalColor = Color.red;
    public Color targetColor = Color.white;

    private Camera playerCamera;
    private CashObject lastHitCash;

    void Start()
    {
        playerCamera = Camera.main;
        if (crosshair != null)
            crosshair.color = normalColor;
    }

    void Update()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        CashObject currentCash = null;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // Check of het een CashObject is
            if (hit.collider.CompareTag(targetTag))
            {
                currentCash = hit.collider.GetComponent<CashObject>();

                // Als het object dichtbij genoeg is, maak het actief/highlight
                if (currentCash != null)
                {
                    currentCash.Highlight(true);
                }
            }
        }

        // Verwijder highlight van vorige als hij niet meer wordt bekeken
        if (lastHitCash != null && lastHitCash != currentCash)
        {
            lastHitCash.Highlight(false);
        }

        lastHitCash = currentCash;

        // Update crosshair
        if (crosshair != null)
            crosshair.color = (currentCash != null) ? targetColor : normalColor;

        // Cash oppakken met E
        if (currentCash != null && Input.GetKeyDown(KeyCode.E))
        {
            currentCash.Collect();
            lastHitCash = null;
        }
    }
}
