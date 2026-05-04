using System;
using System.Collections.Generic;
using System.Linq;
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

    private readonly List<InventoryItem> _items = new();

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
        Debug.Log($"[Inventory] +{item.name}  value:{item.value}  |  Total items: {_items.Count}");
    }

    public IReadOnlyList<InventoryItem> GetItems() => _items;
    public int Count => _items.Count;
    public int TotalValue => _items.Sum(i => i.value); // handy for score/loot tracking
}
