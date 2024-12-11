using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    public struct LiveFireData
    {
        public Vector3 position;
        public Vector3 direction;
        public float speed;
        public float lifeTime;
        public float damage;
    }
    public GameObject ExplosionPrefab;
    public bool ready = false;

    public LiveFireData lfd;

    public LayerMask collisionMask; // LayerMask to specify which layers to check for collision
   
    public void Initialize(Vector3 position, Vector3 direction, float speed, float lifeTime, float damage, LayerMask collisionMask, GameObject explosionPrefab)
    {
        lfd = new LiveFireData { position = position, direction = direction, speed = speed, lifeTime = lifeTime, damage = damage };
        this.collisionMask = collisionMask;
        ExplosionPrefab = explosionPrefab;
        Destroy(gameObject, lfd.lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(ready)
        FireProjectile();
    }

    void FireProjectile()
    {
        Color[] colors = new Color[3] { Color.red, Color.green, Color.blue };
        Debug.DrawLine(transform.position, transform.position + lfd.direction * lfd.speed, colors[Time.frameCount % 3], 3f);

        if (Physics.Raycast(transform.position, lfd.direction, out RaycastHit hit, lfd.speed, collisionMask))
        {
            transform.position = hit.point; // Move to the hit point
            Debug.Log($"Hit: {hit.collider.name}");
            
            // Check if the hit object is a zombie
            hit.collider.TryGetComponent(out ZombieController zombie);
            if (zombie != null)
            {
                zombie.OnRaycastHit(lfd.damage); // Notify the zombie of the raycast hit
            }
            else
            Instantiate(ExplosionPrefab, hit.point, transform.rotation);
            Destroy(gameObject); // Destroy the projectile
        }
        else
        {
            // No collision, move the projectile forward
            transform.position += lfd.direction * lfd.speed;
        }
    }

}
