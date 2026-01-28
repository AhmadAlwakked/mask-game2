using UnityEngine;

public class CashObject : MonoBehaviour
{
    public Outline outline; // Quick Outline component

    private void Start()
    {
        if (outline != null)
            outline.OutlineWidth = 0f; // Standaard geen outline
    }

    // Highlight aan/uit zetten
    public void Highlight(bool value)
    {
        if (outline != null)
            outline.OutlineWidth = value ? 5f : 0f; // 5 = dikte van outline, 0 = uit
    }

    // Cash oppakken
    public void Collect()
    {
        MoneyManager.Instance.AddMoney(5);
        Destroy(gameObject);
    }
}
