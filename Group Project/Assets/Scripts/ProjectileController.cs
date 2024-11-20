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

    public LiveFireData lfd;

    public LayerMask collisionMask; // LayerMask to specify which layers to check for collision
   
    public void Initialize(Vector3 position, Vector3 direction, float speed, float lifeTime, float damage, LayerMask collisionMask)
    {
        lfd = new LiveFireData { position = position, direction = direction, speed = speed, lifeTime = lifeTime, damage = damage };
        this.collisionMask = collisionMask;
        Destroy(gameObject, lfd.lifeTime);

    }

    // Update is called once per frame
    void Update()
    {
        FireProjectile();
    }

    void FireProjectile()
    {
        RaycastHit hit;
        Color[] colors = new Color[3] { Color.red, Color.green, Color.blue };
        Debug.DrawLine(transform.position, transform.position + lfd.direction * lfd.speed, colors[Time.frameCount % 3], 3f);
        
        // Fire a raycast in the specified direction
        if (Physics.Raycast(transform.position, lfd.direction, out hit, lfd.speed, collisionMask))
        {
            transform.position = hit.transform.position;
            Debug.Log("Hit: " + hit.collider.name);
            Destroy(gameObject);
        }
        else
        {
            // No collision, teleport the projectile forward
            transform.position += lfd.direction * lfd.speed;
        }
    }
}