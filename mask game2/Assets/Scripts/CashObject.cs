using UnityEngine;

public class CashObject : MonoBehaviour
{
    public Outline outline;

    [Header("Type")]
    public bool isCash;
    public bool isBattery;

    [Header("Values")]
    public int cashAmount = 5;
    public float batteryAmount = 25f;

    private void Start()
    {
        if (outline != null)
            outline.OutlineWidth = 0f;
    }

    public void Highlight(bool value)
    {
        if (outline != null)
            outline.OutlineWidth = value ? 5f : 0f;
    }

    public void Collect()
    {
        if (isCash)
        {
            MoneyManager.Instance.AddMoney(cashAmount);
        }

        if (isBattery)
        {
            FirstPersonCamera cam = FindObjectOfType<FirstPersonCamera>();
            if (cam != null)
            {
                cam.AddBattery(batteryAmount);
            }
        }

        Destroy(gameObject);
    }
}
