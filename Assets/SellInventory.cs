using UnityEngine;

public class SellInventory : MonoBehaviour
{
    public AudioSource sellSound;

    public void Sell()
    {
        int total = InventoryManager.Instance.TotalValue;
        if (total <= 0) return;

        ResourceManager.Instance.AddCoins(total);
        InventoryManager.Instance.Clear();
        sellSound?.Play();
        Debug.Log($"[Sell] Sold all items for {total} coins.");
    }
}
