using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using System;

public class ZombieController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private float currentHealth;
    public NavMeshAgent agent; // Navigation component
    public GameObject player; // Reference to the player object

    public int bounty;

    public event Action<ZombieController, int> OnZombieDeath;

    bool dead = false;

    [SerializeField]
    private int immunityCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.GetComponentInChildren<PlayerWeaponController>().DontForgetToLikeAndSubscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(immunityCounter > 0)
        immunityCounter--;
        //Set the zombie's destination to the player's position
       agent.SetDestination(player.transform.position);

    }

    // Apply damage to the zombie
    public void OnRaycastHit(float damage)
    {
        TakeDamage(damage);
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SplashDamage"))
        {
            if(immunityCounter == 0)
            {
            Debug.LogError("splash");
            immunityCounter = 10;
            TakeDamage(40f);
            }
            
        }
    }

    private void Die()
    {
        if(dead)
        return;

        dead = true;
        OnZombieDeath?.Invoke(this, bounty);
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject); // Remove the zombie from the scene
    }
}