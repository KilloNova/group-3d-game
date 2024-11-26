using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallBuy : PurchasePoint
{
    

    [SerializeField]
    private GameObject weaponAvailibleToPurchase;

    public bool purchased;
    

    [SerializeField]
    private AudioSource buySound;

    void Start()
    {
        purchasePointType = PurchasePointType.WallBuy;
    }

    public override GameObject BuyWeapon()
    {
        if(!purchased)
        {
        purchased = true;
        buySound.Play();
        return weaponAvailibleToPurchase;
        }
        return null;
    }

    
}
