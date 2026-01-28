using UnityEngine;
using UnityEngine.UI;

public class MaskScalerUI : MonoBehaviour
{
    [Header("UI Masks / Images")]
    public Image mask1;
    public Image mask2;
    public Image mask3;

    [Header("Sizes")]
    public Vector2 normalSize = new Vector2(100, 100);
    public Vector2 enlargedSize = new Vector2(130, 130);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ResetMasks();
            SetMaskSize(mask1, enlargedSize);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetMasks();
            SetMaskSize(mask2, enlargedSize);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResetMasks();
            SetMaskSize(mask3, enlargedSize);
        }
    }

    void ResetMasks()
    {
        SetMaskSize(mask1, normalSize);
        SetMaskSize(mask2, normalSize);
        SetMaskSize(mask3, normalSize);
    }

    void SetMaskSize(Image mask, Vector2 size)
    {
        if (mask != null)
        {
            RectTransform rt = mask.rectTransform; // makkelijker bij UI
            rt.sizeDelta = size;
        }
    }
}
