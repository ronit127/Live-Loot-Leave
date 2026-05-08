using System;
using UnityEngine;

public class StrengthManager : MonoBehaviour
{
    public static StrengthManager Instance { get; private set; }

    public event Action<int> OnStrengthChanged;

    public int StrengthLevel { get; private set; } = 1;

    [Header("Inventory Capacity")]
    public int baseMaxValue = 30;
    public int valuePerLevel = 15;

    public int CurrentMaxInventoryValue => baseMaxValue + (StrengthLevel - 1) * valuePerLevel;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool LevelUp(int coinCost)
    {
        if (!ResourceManager.Instance.SpendCoins(coinCost)) return false;
        StrengthLevel++;
        OnStrengthChanged?.Invoke(StrengthLevel);
        Debug.Log($"[Strength] Level {StrengthLevel} | Max Inventory Value: {CurrentMaxInventoryValue}");
        return true;
    }
}