using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirearmController : MonoBehaviour
{

    public bool inHand;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform bulletSpawnPoint;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private float bulletLifeTime;

    private int fireCycleDelayCounter;

    [SerializeField]
    private int fireCycleDelayCounterMax;

    private int fireReloadDelayCounter;
    [SerializeField]
    private int fireReloadDelayCounterMax;

    [SerializeField]
    private float damage = 1f;

    [SerializeField]
    private LayerMask collisionMask;

    public int _magazineCount;

    [SerializeField]
    private int maxMagazineCount;

    public int _totalBulletCount;

    [SerializeField]
    private Animator animator;

    public enum FireState {
        Ready,
        Reloading,
        Cycling,
        Empty
    }

    [SerializeField]
    private FireState fireState;

    public enum FireModeType {
        Bolt,
        Semi,
        Auto,
        Spread,
        Spread_Auto
    }

    [SerializeField]
    private int spread_shotCount;

    [SerializeField]
    private FireModeType fireModeType;

    public string _weaponName;

    void Update()
    {
        if(!inHand)
            return;


        switch (fireModeType)
        {
            case FireModeType.Auto:
            if(Input.GetKey(KeyCode.Mouse0) && fireState == FireState.Ready)
            {
                Fire();
            }
            break;
            case FireModeType.Semi:
            if(Input.GetKeyDown(KeyCode.Mouse0) && fireState == FireState.Ready)
            {
                Fire();
            }
            break;
            case FireModeType.Spread:
             if(Input.GetKeyDown(KeyCode.Mouse0) && fireState == FireState.Ready)
            {
                FireSpread();
            }
            break;



        }


        if(fireModeType == FireModeType.Auto)
        {
            
        }
        
        if(Input.GetKeyDown(KeyCode.R) && _magazineCount != maxMagazineCount)
        {
            Reload();
        }
    }

    private void FireSpread()
    {
        if(!(fireModeType == FireModeType.Spread && fireModeType == FireModeType.Spread_Auto))
        {
            ProjectileController[] tempBulletList = new ProjectileController[spread_shotCount];
            for (int i = 0; i < spread_shotCount; i++)
            {
                 Vector3 forward = bulletSpawnPoint.forward;
                // Randomize the forward direction slightly
                float spreadAngle = 5.0f; // Maximum spread angle
                float minSpreadAngle = 1.0f; // Minimum spread angle

                // Generate a random spread angle within the range of minSpreadAngle to spreadAngle
                float randomX = UnityEngine.Random.Range(minSpreadAngle, spreadAngle) * (UnityEngine.Random.value > 0.5f ? 1 : -1);
                float randomY = UnityEngine.Random.Range(minSpreadAngle, spreadAngle) * (UnityEngine.Random.value > 0.5f ? 1 : -1);

                // Create a rotation based on the random spread
                Quaternion spreadRotation = Quaternion.Euler(randomX, randomY, 0);

                // Apply the spread rotation to the forward vector
                Vector3 randomizedForward = spreadRotation * forward;
                 GameObject spawnedBullet = Instantiate(bulletPrefab);
                 spawnedBullet.GetComponent<ProjectileController>().Initialize(bulletSpawnPoint.position, randomizedForward, bulletSpeed, bulletLifeTime, damage, collisionMask);
                tempBulletList[i] = spawnedBullet.GetComponent<ProjectileController>();
            }
            for (int i = 0; i < spread_shotCount; i++)
            {
                tempBulletList[i].transform.SetPositionAndRotation(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                tempBulletList[i].ready = true;
            }
            FireEnd();
            
            }
    }
    
    private void FireEnd()
    {
        ResetFireState();   
        AudioSource audioSource = GetComponent<AudioSource>();
        if(fireModeType != FireModeType.Spread)
        audioSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f); // Randomly set pitch between 0.9 and 1.1
        audioSource.Play();       
        _magazineCount--;
        animator.Play("Idle", -1, 0f); // Reset animation to start
        animator.SetTrigger("recoil");
        if(_magazineCount == 0)
        {
            Reload();
            fireState = FireState.Empty;
        }
    }


    void FixedUpdate()
    {
        if(!inHand)
            return;
        HandleFireState();
    }
    private void Reload()
    {
        if(fireState == FireState.Reloading || fireState == FireState.Cycling)
        {
            return;
        }
        Debug.Log("reloading " + _weaponName);
        //if the total bullets remaining exceeds what it needs to fill it up
        if(_totalBulletCount > maxMagazineCount - _magazineCount)
        {
        _totalBulletCount -= maxMagazineCount - _magazineCount;
        _magazineCount = maxMagazineCount;
        }
        //theres not enough bullets to fill the mag to full
        else
        {
            _magazineCount = _totalBulletCount;
            _totalBulletCount = 0;
        }
        fireReloadDelayCounter = 0;
        fireState = FireState.Reloading;
    }
    private void Fire()
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        spawnedBullet.GetComponent<ProjectileController>().Initialize(bulletSpawnPoint.position, bulletSpawnPoint.forward, bulletSpeed, bulletLifeTime, damage, collisionMask);
        spawnedBullet.GetComponent<ProjectileController>().ready = true;
        FireEnd();
    }
    void HandleFireState()
    {
        if(fireState == FireState.Cycling)
        {
            if(fireCycleDelayCounter >= fireCycleDelayCounterMax)
            {
                fireState = FireState.Ready;
            }
            else
            {
            fireCycleDelayCounter++;
            }
        }
        else if(fireState == FireState.Reloading)
        {
            if(fireReloadDelayCounter >= fireReloadDelayCounterMax)
            {
                fireState = FireState.Ready;
            }
            else
            {
            fireReloadDelayCounter++;
            }
        }
        else if(fireState == FireState.Empty && _totalBulletCount != 0)
        {
            Reload();
        }
        
    }
    void ResetFireState()
    {
        fireState = FireState.Cycling;
        fireCycleDelayCounter = 0;
        
    }
}
