using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public struct InventoryItem
{
    public string name;
    public Mesh   mesh;
    public int    value;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public event Action<InventoryItem> OnItemAdded;

    [Header("UI")]
    public TextMeshProUGUI collectiblesText;
    public TextMeshProUGUI totalValueText;

    private readonly List<InventoryItem> _items = new();
    
    void Start()
    {
        UpdateUI();
    }
    
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(InventoryItem item)
    {
        _items.Add(item);
        OnItemAdded?.Invoke(item);
        UpdateUI();
        Debug.Log($"[Inventory] +{item.name}  value:{item.value}  |  Total items: {_items.Count}");
    }

    void UpdateUI()
    {
        if (collectiblesText != null)
            collectiblesText.text = $"Collectibles: {_items.Count}";
        if (totalValueText != null)
            totalValueText.text = $"Value: {TotalValue}";
    }

    public void Clear()
    {
        _items.Clear();
        UpdateUI();
    }

    public IReadOnlyList<InventoryItem> GetItems() => _items;
    public int Count => _items.Count;
    public int TotalValue => _items.Sum(i => i.value);
}
