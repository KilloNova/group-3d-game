using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{

    public delegate void PlayerWeaponsUpdatedHandler();
    public event PlayerWeaponsUpdatedHandler OnPlayerWeaponsUpdated;

    private void NotifyWeaponChange()
    {
        OnPlayerWeaponsUpdated?.Invoke();
    }

    
    [SerializeField]
    public List<GameObject> weapons;

    [SerializeField]
    private int selectedWeapon;

    [SerializeField]
    private PurchasePoint nearbyPurchasePoint;

    public int money;
    // Start is called before the first frame update
    void Start()
    {
        
    } 

    // Update is called once per frame
    void Update()
    {
        for(int i = (int)KeyCode.Alpha1; i < (int)KeyCode.Alpha4; i++)
        if(Input.GetKey((KeyCode)i))
        {
           weaponChange(i - (int)KeyCode.Alpha1);  
        }
        if(Input.GetKey(KeyCode.E))
        {
            BuyWeapon();
        }
    }
    void weaponChange(int index)
    {
        if(index > weapons.Count - 1)
        {
            Debug.LogWarning("player tried to access a weapon slot where there was no weapon");
            return;
        }
        if(selectedWeapon == index)
        {
            return;
        }
        
        weapons[selectedWeapon].SetActive(false);
        weapons[index].SetActive(true);
        weapons[selectedWeapon].GetComponent<FirearmController>().inHand = false;
        weapons[index].GetComponent<FirearmController>().inHand = true;
        
        selectedWeapon = index;

    }


    public void setNearbyPurchasePoint(PurchasePoint point)
    {
        nearbyPurchasePoint = point;
    }

    private void BuyWeapon()
    {
        if(nearbyPurchasePoint == null)
            return; 

        if(nearbyPurchasePoint.GetCost() > money)
        {
            Debug.LogWarning("lacking money.");
            return;
        }
        if(nearbyPurchasePoint.purchasePointType == PurchasePoint.PurchasePointType.WallBuy)
        {
           if(((WallBuy)nearbyPurchasePoint).purchased)
           {
               return;
           }
        }
        money -= nearbyPurchasePoint.GetCost();
        GameObject newWeapon = nearbyPurchasePoint.BuyWeapon();
        weapons.Add(Instantiate(newWeapon, transform));
        weaponChange(weapons.Count - 1);
       
    }

}