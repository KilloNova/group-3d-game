using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using System;
using Unity.VisualScripting;

public class ZombieController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private float currentHealth;
    public NavMeshAgent agent; // Navigation component
    public GameObject player; // Reference to the player object

    public Transform dummyTarget;

    [SerializeField]
    private float attackRange = 2.5f; // Distance within which the zombie attacks

    [SerializeField]
    private float attackInterval = 1.0f; // Time between attacks

    [SerializeField]
    private int damagePerAttack = 1; // Damage dealt per attack

    public int bounty;

    public event Action<ZombieController, int> OnZombieDeath;

    bool dead = false;

    [SerializeField]
    private int immunityCounter = 0;

    private bool isAttacking = false;

    public Vector3 target;

    public PlayerWeaponController playerWeaponController;

    public KillstreakController killstreakController;

    public GameManager gameManager;

    [SerializeField]
    private Animator animator;

    private bool canAttack = true;
    // Start is called before the first frame update
    /*
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.GetComponentInChildren<PlayerWeaponController>().DontForgetToLikeAndSubscribe(this);
    }
    */
    void Start()
    {
        currentHealth = maxHealth;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        playerWeaponController = gameManager.weaponController;
        killstreakController = gameManager.killstreakController;
        dummyTarget = GameObject.FindGameObjectWithTag("DummyTarget").transform;
        // Ensure the player reference is correctly set
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player GameObject with tag 'Player' not found!");
        }
        else
        {
            player.transform.GetComponentInChildren<PlayerWeaponController>()?.DontForgetToLikeAndSubscribe(this);
            playerWeaponController = player.transform.GetComponentInChildren<PlayerWeaponController>();
        }

        // Ensure the NavMeshAgent is correctly set
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"{gameObject.name} is missing a NavMeshAgent component!");
        }

        
    }

    private void StartKillstreak()
    {
        target = dummyTarget.position;
    }

    private void EndKillstreak()
    {
        target = player.transform.position;
    }





    void OnEnable()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        playerWeaponController = gameManager.weaponController;
        killstreakController = gameManager.killstreakController;
        player = gameManager.player;
        playerWeaponController.OnActivateKillstreak += StartKillstreak;
        killstreakController.KillstreakEnd += EndKillstreak;
    }

    void OnDisable()
    {
        playerWeaponController.OnActivateKillstreak -= StartKillstreak;
        killstreakController.KillstreakEnd -= EndKillstreak;

    }
    // Update is called once per frame
    void Update()
    {
        animator = GetComponent<Animator>();
        if(playerWeaponController == null)
        {
            playerWeaponController = FindFirstObjectByType<PlayerWeaponController>();
        }
        if(killstreakController == null)
        {
        killstreakController = FindFirstObjectByType<KillstreakController>();
        }

        if(playerWeaponController.invincible)
        {
            target = dummyTarget.position;
        }
        else
        {
            target = player.transform.position;
        }
        if(immunityCounter > 0)
        immunityCounter--;
        //Set the zombie's destination to the player's position
       agent.SetDestination(target);

       // Check if the zombie is within attack range
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
       if (distanceToPlayer <= attackRange && !isAttacking)
        {

            StartCoroutine(AttackPlayer());
        }

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
        }else
        {
        animator.SetTrigger("damage");
        }
        StartCoroutine(StopAndStartAgent());
    }
    private IEnumerator StopAndStartAgent()
    {
        canAttack = false;
        agent.isStopped = true;
        yield return new WaitForSeconds(0.5f); // adjust the delay as needed
        agent.isStopped = false;
        canAttack = true;
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
        agent.enabled = false;
        animator.SetTrigger("die");
        dead = true;
        OnZombieDeath?.Invoke(this, bounty);
        Debug.Log($"{gameObject.name} has died!");
        Destroy(gameObject, 1.25f); // Remove the zombie from the scene
    }

    private IEnumerator AttackPlayer()
    {
        if(!canAttack)
        {
            yield break;
        }
        isAttacking = true;
        animator = GetComponent<Animator>();

        animator.SetTrigger("attack");

        StartCoroutine(StopAndStartAgent());
        // Damage the player
        Movement playerMovement = player.GetComponent<Movement>();
        if (playerMovement != null)
        {
            playerMovement.TakeDamage(damagePerAttack);
            Debug.Log($"{gameObject.name} attacked the player for {damagePerAttack} damage.");
        }

        // Wait for the attack interval
        yield return new WaitForSeconds(attackInterval);

        isAttacking = false;
    }
}