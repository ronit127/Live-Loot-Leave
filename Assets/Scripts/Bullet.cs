using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 5f;
    public GameObject disintegratePrefab;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // hit box event
        if (other.gameObject.CompareTag("Monster"))
        {
            if (disintegratePrefab != null)
            {
                Instantiate(disintegratePrefab, other.transform.position, Quaternion.identity);
            }

            Destroy(other.gameObject); 
            Debug.Log("Monster Disintegrated!");
        }
        
        Destroy(gameObject);
    }
}