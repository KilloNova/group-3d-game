using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField]
    private PlayerWeaponController playerWeaponController;

    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private float jumpHeight = 2.0f;

    [SerializeField]
    private float gravity = -9.81f;

    [SerializeField]
    private float mouseSensitivity = 100.0f;

    [SerializeField]
    private int playerHealth = 10;

    private int currentHealth;

    private int maxHealth;
    private bool isGameOver = false;

    public float sprintCd = 0f;
    private float sprintTimer = 2f;

    private Vector3 velocity;
    private bool isGrounded;

    private float xRotation = 0f;

    private Transform playerCamera; // Reference to the camera

    public Image ImageComponent;

    private Coroutine regenCoroutine;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        playerWeaponController = transform.GetComponentInChildren<PlayerWeaponController>();
        playerCamera = Camera.main.transform; // Use Camera.main to get the main camera

        currentHealth = playerHealth;
        maxHealth = currentHealth;

    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        if (regenCoroutine == null)
        {
            regenCoroutine = StartCoroutine(RegenerateHP());
        }

    }

    private IEnumerator RegenerateHP() {
        while (currentHealth < maxHealth)
        {
            currentHealth += 1; // Increase HP by 1
            Debug.Log("Current HP: " + maxHealth);
            yield return new WaitForSeconds(1);
        }
        updateHealthHud();

    }

    void HandleMovement()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to ensure the player stays grounded
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        if (Input.GetKey(KeyCode.LeftShift) && sprintCd <= 0f)
        {
            characterController.Move(move * (speed + speed) * Time.deltaTime);
            sprintTimer -= Time.deltaTime;
            if (sprintTimer <= 0f)
            {
                sprintCd = 2f;
            }
        }
        else
        {
            characterController.Move(move * speed * Time.deltaTime);
            sprintCd -= Time.deltaTime;
            if (sprintCd <= 0)
            {
                sprintTimer = 2f;
                sprintCd = 0;
            }
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp the vertical rotation

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotate the camera vertically
        transform.Rotate(Vector3.up * mouseX); // Rotate the player horizontally
    }

    public void TakeDamage(int damage)
    {
        if(playerWeaponController.invincible)
        return;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }

<<<<<<< HEAD
        updateHealthHud();
        
    }

    void updateHealthHud(){
=======
>>>>>>> 0a10480f1cb902720551883b6ed0b46e087645e6
            float healthPercentage = Mathf.Clamp01((float)currentHealth / maxHealth);
            float alpha = 1f - healthPercentage;
            Color color = ImageComponent.color;
            color.a = alpha;
            ImageComponent.color = color;
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        isGameOver = true;

        
        GameOver();
    }

    private void GameOver()
    {
        // Stop all player movement and input
        velocity = Vector3.zero;

        // Lock the cursor
        Cursor.lockState = CursorLockMode.None;

        // Optionally pause the game
        Time.timeScale = 0;

        // Display a game-over UI (add your own logic or link to a UIManager)
        Debug.Log("Game Over! Display game-over screen here.");

        // Example: Restart the game (optional, replace with your desired behavior)
        // Uncomment if you want to allow restarting immediately for testing
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
