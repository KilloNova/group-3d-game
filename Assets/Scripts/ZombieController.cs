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
        if (gameObject.CompareTag("PlayerAttack"))
        {
            health -= damage;

            // Check if health has reached zero
            if (health <= 0)
            {
                Destroy(gameObject); // Destroy the enemy
            }
        }
    }

/*
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "PlayerTestObject")
        {
            HeartScript.TakeDamage(damageAmount);
        }
        //Check if collided with the player

        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            health -= 1;

            // Destroy the attack object upon collision
            Destroy(collision.gameObject);

            // Check if health has reached zero
            if (health <= 0)
            {
                Destroy(gameObject); // Destroy the enemy
            }
        }
    }
    */
}
