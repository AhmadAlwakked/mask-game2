using UnityEngine;

[ExecuteAlways]
public class HeatObject : MonoBehaviour
{
    [Header("Heat Settings")]
    [Range(0f, 1f)]
    public float heat = 0.0f; // 0 = koud/blauw, 1 = warm/rood

    [Header("Materials")]
    public Material standardMaterial; // optioneel
    public Material heatMaterial;     // optioneel

    [Header("Optional Light")]
    public Light objectLight;         // het Light component
    public Color heatLightColor = Color.white; // kleur van Light bij HeatVision aan

    private Renderer objectRenderer;
    private bool heatActive = false;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        UpdateMaterial(false); // start zonder HeatVision
        UpdateHeat();
    }

    void Update()
    {
        UpdateHeat();
        UpdateLight();
    }

    public void UpdateHeat()
    {
        if (objectRenderer == null) return;
        if (heatMaterial == null) return;

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        objectRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Heat", heat);
        objectRenderer.SetPropertyBlock(mpb);
    }

    // Zet HeatVision aan/uit
    public void UpdateMaterial(bool active)
    {
        heatActive = active;

        // Materiaal switch
        if (objectRenderer != null)
        {
            if (heatActive && heatMaterial != null)
                objectRenderer.material = heatMaterial;
            else if (!heatActive && standardMaterial != null)
                objectRenderer.material = standardMaterial;
        }

        // Licht switch
        UpdateLight();
    }

    private void UpdateLight()
    {
        if (objectLight == null) return;

        objectLight.intensity = 5000f; // altijd

        // Kleur: HeatVision aan → heatLightColor, uit → wit
        objectLight.color = heatActive ? heatLightColor : Color.white;
    }
}
