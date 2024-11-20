using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirearmController : MonoBehaviour
{

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
    private float damage;

    [SerializeField]
    private LayerMask collisionMask;

    [SerializeField]
    private int magazineCount;

    [SerializeField]
    private int maxMagazineCount;

    [SerializeField]
    private int totalBulletCount;

    public enum FireState {
        Ready,
        Reloading,
        Cycling,
        Empty
    }

    [SerializeField]
    private FireState fireState;

    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0) && fireState == FireState.Ready)
        {
            Fire();
        }
        if(Input.GetKeyDown(KeyCode.R) && magazineCount != maxMagazineCount)
        {
            Reload();
        }
    }

    

    void FixedUpdate()
    {
        HandleFireState();
    }
    private void Reload()
    {
        if(fireState == FireState.Reloading || fireState == FireState.Cycling)
        {
            return;
        }
        Debug.Log("reloading");
        //if the total bullets remaining exceeds what it needs to fill it up
        if(totalBulletCount > maxMagazineCount - magazineCount)
        {
        totalBulletCount -= maxMagazineCount - magazineCount;
        magazineCount = maxMagazineCount;
        }
        //theres not enough bullets to fill the mag to full
        else
        {
            magazineCount = totalBulletCount;
            totalBulletCount = 0;
        }
        fireReloadDelayCounter = 0;
        fireState = FireState.Reloading;
    }
    private void Fire()
    {
        GameObject spawnedBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        spawnedBullet.GetComponent<ProjectileController>().Initialize(bulletSpawnPoint.position, bulletSpawnPoint.forward, bulletSpeed, bulletLifeTime, damage, collisionMask);
        ResetFireState();   
        GetComponent<AudioSource>().Play();
        magazineCount--;
        if(magazineCount == 0)
        {
            Reload();
            fireState = FireState.Empty;
        }
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
        else if(fireState == FireState.Empty && totalBulletCount != 0)
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
