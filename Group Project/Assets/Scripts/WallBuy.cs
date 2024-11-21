using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallBuy : MonoBehaviour
{
    [SerializeField]
    private BoxCollider buyZone;

    [SerializeField]
    private GameObject weaponAvailibleToPurchase;

    [SerializeField]
    private int cost;

    public bool purchased;
    
    public bool inBuyZone = false;

    private GameObject player;

    [SerializeField]
    private AudioSource buySound;

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.Equals(player))
        {
            inBuyZone = true;
            other.GetComponentInChildren<PlayerWeaponController>().setNearbyWallBuy(this);
        }
       
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.Equals(player))
        {
            inBuyZone = false;
            other.GetComponentInChildren<PlayerWeaponController>().setNearbyWallBuy(null);
        }
    }

    public int getCost()
    {
        return cost;
    }

    public GameObject BuyWeapon()
    {
        if(!purchased)
        {
        purchased = true;
        buySound.Play();
        return weaponAvailibleToPurchase;
        }
        return null;
    }

    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
