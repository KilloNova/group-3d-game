using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireController : MonoBehaviour
{

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform bulletSpawnPoint;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private float bulletLifeTime;

    private int fireDelayCounter;

    [SerializeField]
    private int fireDelayCounterMax;

    [SerializeField]
    private float damage;

    [SerializeField]
    private LayerMask collisionMask;


    public enum FireState {
        Ready,
        Reloading
    }

    [SerializeField]
    private FireState fireState;

    // Update is called once per frame
    void Update()
    {
        HandleFireState();
        if(Input.GetKey(KeyCode.Mouse0) && fireState == FireState.Ready)
        {
            GameObject spawnedBullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            spawnedBullet.GetComponent<ProjectileController>().Initialize(bulletSpawnPoint.position, bulletSpawnPoint.forward, bulletSpeed, bulletLifeTime, damage, collisionMask);
            fireState = FireState.Reloading;
            fireDelayCounter = 0;
        }
    }

    void HandleFireState()
    {
        if(fireState == FireState.Reloading)
        {
            if(fireDelayCounter >= fireDelayCounterMax)
            {
                fireState = FireState.Ready;
            }
            else
            {
            fireDelayCounter++;
            }
        }
    }
}
