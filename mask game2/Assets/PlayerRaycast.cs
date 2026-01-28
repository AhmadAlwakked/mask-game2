using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycast : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float maxDistance = 5f;
    public string targetTag = "Cash";

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
            if (hit.collider.CompareTag(targetTag))
                currentCash = hit.collider.GetComponent<CashObject>();
        }

        // Alleen update als het target veranderd is
        if (currentCash != lastHitCash)
        {
            if (lastHitCash != null)
                lastHitCash.Highlight(false);

            if (currentCash != null)
                currentCash.Highlight(true);

            lastHitCash = currentCash;
        }

        // Update crosshair
        if (crosshair != null)
            crosshair.color = (currentCash != null) ? targetColor : normalColor;

        // Cash oppakken
        if (currentCash != null && Input.GetKeyDown(KeyCode.E))
        {
            currentCash.Collect();
            lastHitCash = null;
        }
    }
}
