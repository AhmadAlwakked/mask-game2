using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [SerializeField] private int startMoney = 100;
    public int CurrentMoney { get; private set; }

    [Header("UI")]
    [SerializeField] private TMP_Text moneyText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CurrentMoney = startMoney;
            UpdateMoneyUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddMoney(int amount)
    {
        CurrentMoney += amount;
        UpdateMoneyUI();
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = "Geld: $" + CurrentMoney;
    }
}
