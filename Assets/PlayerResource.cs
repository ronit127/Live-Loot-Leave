using UnityEngine;
using UnityEngine.UI;

public class PlayerResource : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Player defeated!");
            // Add defeat logic
        }
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }
}