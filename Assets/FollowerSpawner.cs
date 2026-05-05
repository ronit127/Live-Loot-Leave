using UnityEngine;

public class FollowerSpawner : MonoBehaviour
{
    public GameObject followerPrefab;
    public float spawnInterval = 25f;
    public AudioSource spawnSound;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), spawnInterval, spawnInterval);
    }

    void Spawn()
    {
        if (followerPrefab != null)
            Instantiate(followerPrefab, transform.position, transform.rotation);

        spawnSound?.Play();
    }
}
