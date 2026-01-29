using UnityEngine;

[ExecuteAlways]
public class HeatObject : MonoBehaviour
{
    [Header("Heat Settings")]
    [Range(0f, 1f)]
    public float heat = 0.0f;

    [Header("Materials")]
    public Material standardMaterial;
    public Material heatMaterial;

    [Header("Optional Light")]
    public Light objectLight;

    [Tooltip("Zet AAN als dit object een zaklamp is")]
    public bool isFlashlight = false;

    [Header("Light Settings")]
    public Color heatLightColor = new Color(1f, 0f, 1f); // paars bij mask2/HeatVision
    public float normalIntensity = 1000f;
    public float flashlightIntensity = 5000f;

    private Renderer objectRenderer;
    private bool heatActive = false;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        ApplyMaterial();
        ApplyLight();
    }

    void Update()
    {
        UpdateHeat();
        ApplyLight();
    }

    public void UpdateHeat()
    {
        if (objectRenderer == null || heatMaterial == null) return;

        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        objectRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat("_Heat", heat);
        objectRenderer.SetPropertyBlock(mpb);
    }

    public void UpdateMaterial(bool active)
    {
        heatActive = active;
        ApplyMaterial();
        ApplyLight();
    }

    private void ApplyMaterial()
    {
        if (objectRenderer == null) return;

        if (heatActive && heatMaterial != null)
            objectRenderer.material = heatMaterial;
        else if (!heatActive && standardMaterial != null)
            objectRenderer.material = standardMaterial;
    }

    private void ApplyLight()
    {
        if (objectLight == null) return;

        // ✅ Laat Light.enabled ongemoeid! Doet flashlight script
        // Pas alleen intensiteit en kleur aan

        objectLight.intensity = isFlashlight ? flashlightIntensity : normalIntensity;

        // Kleur: alleen aanpassen als HeatVision actief
        objectLight.color = heatActive ? heatLightColor : Color.white;
    }
}
