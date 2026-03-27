using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the enemy prefab
    public int numberOfEnemies = 5; // Number of enemies to spawn
    public int spawnRadius = 10; // Radius within which to spawn enemies

    private bool hasSpawned = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasSpawned && other.CompareTag("Player"))
        {
            Spawn();
            hasSpawned = true; // Ensure enemies are spawned only once
        }
    }
    void Spawn()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            
        }
    }

    // Update is called once per frame
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
