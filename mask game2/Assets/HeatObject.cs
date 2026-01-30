using UnityEngine;

[ExecuteAlways]
public class HeatObject : MonoBehaviour
{
    [Header("Heat Settings")]
    [Range(0f, 1f)]
    public float heat = 0.0f;

    [Header("Materials")]
    public Material standardMaterial; // fallback normaal materiaal
    public Material heatMaterial;

    [Header("Optional Light")]
    public Light objectLight;
    public bool isFlashlight = false;
    public Color heatLightColor = new Color(1f, 0f, 1f);
    public float normalIntensity = 1000f;
    public float flashlightIntensity = 5000f;

    [Header("Optional Terrain Support")]
    public Terrain terrain;

    private Renderer objectRenderer;
    private bool heatActive = false;

    // Nieuwe variabele om origineel terrain materiaal te onthouden
    private Material terrainOriginalMaterial;

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();

        // Onthoud originele terrain material
        if (terrain != null)
        {
            terrainOriginalMaterial = terrain.materialTemplate;
        }

        heatActive = false;
        ApplyMaterial();
        ApplyLight();
        UpdateHeat();
    }

    void Update()
    {
        UpdateHeat();
        ApplyLight();
    }

    public void UpdateHeat()
    {
        if (!heatActive) return;

        if (objectRenderer != null && heatMaterial != null)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            objectRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Heat", heat);
            objectRenderer.SetPropertyBlock(mpb);
        }

        if (terrain != null && heatMaterial != null)
        {
            terrain.materialTemplate = heatMaterial;
            terrain.materialTemplate.SetFloat("_Heat", heat);
        }
    }

    public void UpdateMaterial(bool active)
    {
        heatActive = active;
        ApplyMaterial();
        ApplyLight();
    }

    private void ApplyMaterial()
    {
        if (objectRenderer != null)
        {
            if (heatActive && heatMaterial != null)
                objectRenderer.material = heatMaterial;
            else if (!heatActive && standardMaterial != null)
                objectRenderer.material = standardMaterial;
        }

        if (terrain != null)
        {
            if (heatActive && heatMaterial != null)
            {
                terrain.materialTemplate = heatMaterial;
                terrain.materialTemplate.SetFloat("_Heat", heat);
            }
            else
            {
                // Zet terug naar origineel materiaal
                if (terrainOriginalMaterial != null)
                    terrain.materialTemplate = terrainOriginalMaterial;
                else if (standardMaterial != null)
                    terrain.materialTemplate = standardMaterial;
            }
        }
    }

    private void ApplyLight()
    {
        if (objectLight == null) return;

        objectLight.intensity = isFlashlight ? flashlightIntensity : normalIntensity;
        objectLight.color = heatActive ? heatLightColor : Color.white;
    }
}
