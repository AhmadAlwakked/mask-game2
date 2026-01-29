using UnityEngine;

[ExecuteAlways]
public class HeatObject : MonoBehaviour
{
    [Header("Heat Settings")]
    [Range(0f, 1f)]
    public float heat = 0.0f; // 0 = koud/blauw, 1 = warm/rood

    [Header("Materials")]
    public Material standardMaterial; // normale look
    public Material heatMaterial;     // HeatVision shader

    private Renderer objectRenderer;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        UpdateMaterial(false); // start met standaard materiaal
        UpdateHeat();
    }

    void OnValidate()
    {
        UpdateHeat();
    }

    // Update per-object heat waarde in hot material
    public void UpdateHeat()
    {
        if (objectRenderer == null) objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null && heatMaterial != null)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Heat", heat);
            objectRenderer.SetPropertyBlock(mpb);
        }
    }

    // Schakel tussen hot en normaal materiaal
    public void UpdateMaterial(bool heatActive)
    {
        if (objectRenderer == null) objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null) return;

        if (heatActive && heatMaterial != null)
        {
            objectRenderer.material = heatMaterial;
            UpdateHeat(); // zorg dat heat value klopt
        }
        else if (!heatActive && standardMaterial != null)
        {
            objectRenderer.material = standardMaterial;
        }
    }
}
