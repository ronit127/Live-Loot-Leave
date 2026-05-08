using System;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    public int maxHealth = 200;
    public TextMeshProUGUI healthText;

    public event Action<int> OnHealthChanged;
    public event Action OnDeath;

    public int Health { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Health = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        if (Health <= 0) return;

        Health = Mathf.Max(0, Health - amount);
        OnHealthChanged?.Invoke(Health);
        UpdateUI();

        if (Health <= 0)
        {
            OnDeath?.Invoke();
            Debug.Log("[Health] Player died.");
        }
    }

    void UpdateUI()
    {
        if (healthText != null)
            healthText.text = $"HP: {Health} / {maxHealth}";
    }

    // heal when sell
    public void Heal(int amount)
    {
        if (Health < 0) return;
        Health = Mathf.Min(maxHealth, Health + amount);
        OnHealthChanged?.Invoke(Health);
        UpdateUI();
        Debug.Log($"[Health] Healed by {amount}. HP: {Health}/{maxHealth}");
    }
}
