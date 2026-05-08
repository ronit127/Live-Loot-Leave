using System;
using UnityEngine;

public class AgilityManager : MonoBehaviour
{
    public static AgilityManager Instance { get; private set; }

    public event Action<int> OnAgilityChanged;

    public int AgilityLevel { get; private set; } = 1;

    [Header("Base Stats")]
    public float baseSpeed = 3f;
    public float baseJumpHeight = 1f;
    public float speedPerLevel = 0.5f;
    public float jumpHeightPerLevel = 0.1f;

    public float CurrentSpeed => baseSpeed + (AgilityLevel - 1) * speedPerLevel;
    public float CurrentJumpHeight => baseJumpHeight + (AgilityLevel - 1) * jumpHeightPerLevel;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool LevelUp(int coinCost)
    {
        if (!ResourceManager.Instance.SpendCoins(coinCost)) return false;
        AgilityLevel++;
        OnAgilityChanged?.Invoke(AgilityLevel);
        Debug.Log($"[Agility] Level {AgilityLevel} | Speed: {CurrentSpeed} | Jump: {CurrentJumpHeight}");
        return true;
    }
}