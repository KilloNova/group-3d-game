using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public float health = 5;
    public NavMeshAgent agent;

    public GameObject player;

    public 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);
    }
    public void impact(float damage)
    {
        if (gameObject.CompareTag("Projectile"))
        {
            health -= damage;

            // Check if health has reached zero
            if (health <= 0)
            {
                Destroy(gameObject); // Destroy the enemy
            }
        }
    } 



    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is a projectile
        if (other.CompareTag("Projectile"))
        {
            Debug.Log("Projectile hit zombie!"); // Add debugging

            // Access the damage value from the projectile
            ProjectileController projectile = other.GetComponent<ProjectileController>();
            if (projectile != null)
            {
                impact(projectile.lfd.damage); // Apply damage to the zombie
            }

            // Destroy the projectile after it hits the zombie
            Destroy(other.gameObject);
        }
    }


}