using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    public int ammo;

    public PlayerWeaponController playerWeaponController;

    public int currentAmmoCount;
    
    public int totalBulletCount;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentAmmoCount = playerWeaponController.currentAmmoCount;
    }
}
