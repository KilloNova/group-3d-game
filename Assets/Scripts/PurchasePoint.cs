using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PurchasePoint : MonoBehaviour
{

    public bool inBuyZone = false;
    [SerializeField]
    private int cost;

    [SerializeField]
    private BoxCollider buyZone;

    public AudioSource buySound;

    private GameObject player;

    public enum PurchasePointType {
        WallBuy,
        MysteryBox
    }

    public PurchasePointType purchasePointType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.Equals(player))
        {
            inBuyZone = false;
            other.GetComponentInChildren<PlayerWeaponController>().setNearbyPurchasePoint(null);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.Equals(player))
        {
            inBuyZone = true;
            other.GetComponentInChildren<PlayerWeaponController>().setNearbyPurchasePoint(this);
        }
       
    }

    public int GetCost()
    {
        return cost;
    }

    public abstract GameObject BuyWeapon();

}
