using TMPro;
using UnityEngine;

public class CoinDisplay : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public int coinsRequired = 100;

    void OnEnable()
    {
        ResourceManager.Instance.OnCoinsChanged += UpdateDisplay;
        UpdateDisplay(ResourceManager.Instance.Coins);
    }

    void OnDisable()
    {
        ResourceManager.Instance.OnCoinsChanged -= UpdateDisplay;
    }

    void UpdateDisplay(int coins)
    {
        if (coinText != null)
            coinText.text = $"{coins} / {coinsRequired} Coins Required";
    }
}
