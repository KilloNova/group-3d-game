using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    public PlayerWeaponController playerWeaponController; // Reference to the weapon controller
    public TextMeshProUGUI ammoText; // Reference to the TextMeshPro component
    public TextMeshProUGUI killText;

    public TextMeshProUGUI inventoryText;
    public Image progressBar;
    void Start()
    {
        // Find the PlayerWeaponController if not already assigned
        if (playerWeaponController == null)
        {
            playerWeaponController = GameObject.FindGameObjectWithTag("WeaponController")
                .GetComponent<PlayerWeaponController>();
        }
    }

    void Update()
    {
        // Check if the playerWeaponController exists and update the text
        if (playerWeaponController != null && ammoText != null)
        {
            ammoText.text = $"{playerWeaponController.currentAmmoCount}/{playerWeaponController.totalBulletCount}";
            inventoryText.text = string.Join(" ", playerWeaponController.currentWeapons);

        }
        if(!playerWeaponController.invincible)
        progressBar.fillAmount = (float)playerWeaponController.killAmount / playerWeaponController.maxKillAmount;
        if(progressBar.fillAmount >= .99f || playerWeaponController.invincible)
        {
            killText.enabled = true;
            StartFlashing();
        }
        else
        {
            killText.enabled = false;
            StopFlashing();
        }
    }

    private float flashDuration = 0.5f;    // Duration of one flash cycle
    private float flashSpeed = 2f;         // Speed of the flash effect
    private bool isFlashing = false;
    private float flashTimer = 0f;

    public void StartFlashing()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            flashTimer = 0f;
            InvokeRepeating("FlashProgressBar", 0f, 0.016f); // Approximately 60fps
        }
    }

    private void FlashProgressBar()
    {
        if (!isFlashing) return;

        flashTimer += Time.deltaTime;
        float alpha = Mathf.PingPong(flashTimer * flashSpeed, 1f);
        Color currentColor = progressBar.color;
        progressBar.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

        if (flashTimer >= flashDuration)
        {
            StopFlashing();
        }
    }

    private void StopFlashing()
    {
        isFlashing = false;
        CancelInvoke("FlashProgressBar");
        Color currentColor = progressBar.color;
        progressBar.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1f); // Reset to full opacity
    }
}