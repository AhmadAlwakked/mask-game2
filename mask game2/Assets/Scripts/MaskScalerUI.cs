using UnityEngine;
using UnityEngine.UI;

public class MaskScalerUI : MonoBehaviour
{
    [Header("UI Masks / Images")]
    public Image mask1;
    public Image mask2;
    public Image mask3;

    [Header("Masker GameObjects")]
    public GameObject mask1Object;
    public GameObject mask2Object;
    public GameObject mask3Object;

    [Header("Sizes")]
    public Vector2 normalSize = new Vector2(100, 100);
    public Vector2 enlargedSize = new Vector2(130, 130);

    [Header("Panel Settings")]
    public Image panel; // Panel dat van kleur verandert
    public Color mask1Color = new Color(1f, 0f, 0f, 0.5f); // Rood, 50% transparant
    public Color mask2Color = new Color(0f, 1f, 0f, 0.5f); // Groen, 50% transparant
    public Color mask3Color = new Color(0f, 0f, 1f, 0.5f); // Blauw, 50% transparant

    [Header("Mask States")]
    public bool isMask1Active = false;
    public bool isMask2Active = false;
    public bool isMask3Active = false;

    void Start()
    {
        // Mask1 automatisch equipt bij spelstart
        EquipMask(mask1, mask1Color, mask1Object);
    }

    void Update()
    {
        // Keys om masks te switchen
        if (Input.GetKeyDown(KeyCode.Alpha1))
            EquipMask(mask1, mask1Color, mask1Object);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            EquipMask(mask2, mask2Color, mask2Object);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            EquipMask(mask3, mask3Color, mask3Object);
    }

    // Equip een masker: vergroot, verander panelkleur, activeer masker object en update bool
    void EquipMask(Image mask, Color panelColor, GameObject maskObject)
    {
        ResetMasks(); // reset alle masks en bools
        SetMaskSize(mask, enlargedSize);
        SetPanelColor(panelColor);
        ActivateMaskObject(maskObject);
        SetMaskBool(maskObject, true);
    }

    // Zet alle masks terug naar normale grootte en zet masker objects uit
    void ResetMasks()
    {
        SetMaskSize(mask1, normalSize);
        SetMaskSize(mask2, normalSize);
        SetMaskSize(mask3, normalSize);

        if (mask1Object != null) mask1Object.SetActive(false);
        if (mask2Object != null) mask2Object.SetActive(false);
        if (mask3Object != null) mask3Object.SetActive(false);

        // Zet alle bools uit
        isMask1Active = false;
        isMask2Active = false;
        isMask3Active = false;
    }

    void SetMaskSize(Image mask, Vector2 size)
    {
        if (mask != null)
            mask.rectTransform.sizeDelta = size;
    }

    void SetPanelColor(Color color)
    {
        if (panel != null)
            panel.color = color;
    }

    void ActivateMaskObject(GameObject maskObject)
    {
        if (maskObject != null)
            maskObject.SetActive(true);
    }

    void SetMaskBool(GameObject maskObject, bool state)
    {
        if (maskObject == mask1Object) isMask1Active = state;
        else if (maskObject == mask2Object) isMask2Active = state;
        else if (maskObject == mask3Object) isMask3Active = state;
    }
}
