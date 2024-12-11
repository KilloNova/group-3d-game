using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject zombiePrefab;

    [SerializeField]
    private float zombieInterval = 3.5f;

    [SerializeField]
    private float spawnRadius = 50f; // Radius around the spawner to spawn enemies

    private void Start()
    {
        StartCoroutine(SpawnEnemy(zombieInterval));
    }

    private IEnumerator SpawnEnemy(float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // Generate a random position within a circle around the spawner
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = new Vector3(transform.position.x + randomCircle.x, 100f, transform.position.z + randomCircle.y);

            // Use a raycast to determine the ground level (y)
            RaycastHit hit;
            if (Physics.Raycast(spawnPosition, Vector3.down, out hit, Mathf.Infinity))
            {
                spawnPosition.y = hit.point.y; // Set y to the ground level
            }
            else
            {
                spawnPosition.y = 0f; // Default to 0 if no ground is hit
            }

            // Spawn the zombie at the calculated position
            Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        }
    }
}