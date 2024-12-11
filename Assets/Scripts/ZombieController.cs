using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public float health = 5; // Zombie's health
    public NavMeshAgent agent; // Navigation component
    public GameObject player; // Reference to the player object

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Set the zombie's destination to the player's position
        agent.SetDestination(player.transform.position);
    }

    // Apply damage to the zombie
    public void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log($"{gameObject.name} took {damage} damage! Remaining health: {health}");

        // Check if the zombie's health reaches zero
        if (health <= 0)
        {
            Die();
        }
    }

    // Handle zombie death
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject); // Destroy the zombie object
    }

    // Detect collisions with projectiles
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Debug.Log("Projectile hit zombie!");

            // Get the damage value from the projectile
            ProjectileController projectile = other.GetComponent<ProjectileController>();
            if (projectile != null)
            {
                TakeDamage(projectile.lfd.damage); // Apply damage to the zombie
            }

            // Destroy the projectile after it hits
            Destroy(other.gameObject);
        }
    }

    // Handle being hit by a raycast
    public void OnRaycastHit(float damage)
    {
        Debug.Log($"Zombie {gameObject.name} hit by raycast!");
        TakeDamage(damage); // Apply raycast damage
    }
}