using TMPro;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public PlayerWeaponController playerWeaponController; // Reference to the weapon controller
    public TextMeshProUGUI ammoText; // Reference to the TextMeshPro component

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
        }
    }
}