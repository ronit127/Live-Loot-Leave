using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MonsterSystem : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject monsterPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 180f;
    public int maxMonsters = 5;

    [Header("Path Following")]
    public Transform[] waypoints;
    public float moveSpeed = 2f;

    [Header("Enemy Settings")]
    public string playerTag = "Player";
    public int damagePerHit = 10;
    public Transform playerTransform;
    public float vicinityDistance = 10f;

    [Header("Resource Simulation")]
    public PlayerResource playerResource;

    [Header("Loss Timer")]
    public TextMeshProUGUI timerText;
    public float gameDuration = 60f;

    [Header("Sound")]
    public AudioSource audioSource;

    private float timer;
    private bool gameOver;
    private bool timerStarted = false;
    private List<GameObject> activeMonsters = new List<GameObject>();

    void Start()
    {
        timer = gameDuration;
        UpdateTimerUI();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        InvokeRepeating("SpawnMonster", 1f, spawnInterval);
        // Timer starts when player is near a monster
    }

    void Update()
    {
        if (!timerStarted && playerTransform != null && activeMonsters.Count > 0)
        {
            foreach (GameObject monster in activeMonsters)
            {
                if (monster != null && Vector3.Distance(playerTransform.position, monster.transform.position) < vicinityDistance)
                {
                    timerStarted = true;
                    StartCoroutine(TimerCountdown());
                    break;
                }
            }
        }

        // Clean up destroyed monsters
        activeMonsters.RemoveAll(monster => monster == null);
    }

    void SpawnMonster()
    {
        if (spawnPoints.Length == 0 || monsterPrefab == null)
            return;

        // Destroy previous monsters
        foreach (GameObject monster in activeMonsters)
        {
            if (monster != null)
                Destroy(monster);
        }
        activeMonsters.Clear();

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject newMonster = Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
        activeMonsters.Add(newMonster);

        Debug.Log("Spawned monster. Total active: " + activeMonsters.Count);

        // Add path following
        MonsterPathFollower pathFollower = newMonster.AddComponent<MonsterPathFollower>();
        pathFollower.waypoints = waypoints;
        pathFollower.moveSpeed = moveSpeed;

        // Add enemy behavior
        MonsterEnemy enemy = newMonster.AddComponent<MonsterEnemy>();
        enemy.playerTag = playerTag;
        enemy.damage = damagePerHit;
        enemy.playerResource = playerResource;

        // Play spawn sound
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    IEnumerator TimerCountdown()
    {
        while (timer > 0 && !gameOver)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            UpdateTimerUI();

            if (timer <= 0)
            {
                GameOver();
            }
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + Mathf.Ceil(timer).ToString();
        }
    }

    void GameOver()
    {
        gameOver = true;
        Debug.Log("Game Over! Time's up.");
        // Add game over logic here
    }
}

public class MonsterPathFollower : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 2f;

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0)
            return;

        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}

public class MonsterEnemy : MonoBehaviour
{
    public string playerTag = "Player";
    public int damage = 10;
    public PlayerResource playerResource;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            if (playerResource != null)
            {
                playerResource.TakeDamage(damage);
            }
        }
    }
}