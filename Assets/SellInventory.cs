using UnityEngine;

public class SellInventory : MonoBehaviour
{
    public AudioSource sellSound;
    public int healthRestoreOnSell = 50;

    public void Sell()
    {
        int total = InventoryManager.Instance.TotalValue;
        if (total <= 0) return;

        ResourceManager.Instance.AddCoins(total);
        InventoryManager.Instance.Clear();
        sellSound?.Play();

        // heal when sell
        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.Heal(healthRestoreOnSell);

        Debug.Log($"[Sell] Sold all items for {total} coins.");
    }
}
