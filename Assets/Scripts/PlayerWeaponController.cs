using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{

    public delegate void PlayerWeaponsUpdatedHandler();
    public event PlayerWeaponsUpdatedHandler OnPlayerWeaponsUpdated;

    private void NotifyWeaponChange()
    {
        OnPlayerWeaponsUpdated?.Invoke();
    }

    public void DontForgetToLikeAndSubscribe(ZombieController zombie)
    {
        zombie.OnZombieDeath += PlayerKilledZombie;
    }

    public delegate void ActivateKillstreakHandler();
    // Declare the event using the delegate
    public event ActivateKillstreakHandler OnActivateKillstreak;

    // Method to invoke the ActivateKillstreak event
    
    [SerializeField]
    private bool killstreak = false;

    public int currentAmmoCount;

    public int totalBulletCount;

    public string currentWeaponName;

    public List<string> currentWeapons;


    [SerializeField]
    public List<GameObject> weapons;

    [SerializeField]
    private int selectedWeapon;

    [SerializeField]
    private Transform weaponSpawnPoint;

    [SerializeField]
    private PurchasePoint nearbyPurchasePoint;

    public int money;

    [SerializeField]
    private KillstreakController killstreakController;

    [SerializeField]
    private Camera killstreakCamera;

    bool invincible = true;
    // Start is called before the first frame update
    void Start()
    {
        weapons.Capacity = 4;
        killstreakController = GameObject.FindFirstObjectByType<KillstreakController>();
    } 

    private void OnEnable()
    {
        killstreakController.KillstreakEnd += EndKillstreak;
    }

    private void OnDisable()
    {
        killstreakController.KillstreakEnd -= EndKillstreak;
    }

    private void EndKillstreak()
    {
        Debug.LogError("ended killstreak");
        weapons[selectedWeapon].GetComponent<FirearmController>().inHand = true;
        invincible = false;
        transform.root.GetComponent<Movement>().enabled = true;
        killstreak = false;
        killstreakCamera.gameObject.SetActive(false);
    }

    public void ActivateKillstreak()
    {
        killstreakCamera.gameObject.SetActive(true);
        transform.root.GetComponent<Movement>().enabled = false;
        killstreak = true;
        invincible = true;
        OnActivateKillstreak?.Invoke();
        foreach (var item in weapons)
        {
            item.GetComponent<FirearmController>().inHand = false;
        }

    }

    private void PlayerKilledZombie(ZombieController zombie, int bounty)
    {
        money += bounty;
    }
    // Update is called once per frame
    void Update()
    {
        if(killstreak)
        return;
        for(int i = (int)KeyCode.Alpha1; i < (int)KeyCode.Alpha4; i++)
        if(Input.GetKey((KeyCode)i))
        {
           weaponChange(i - (int)KeyCode.Alpha1);  
        }
        if(Input.GetKey(KeyCode.E))
        {
            BuyWeapon();
        }
        FirearmController currentWeapon = weapons[selectedWeapon].GetComponent<FirearmController>();
        currentAmmoCount = currentWeapon._magazineCount;
        totalBulletCount = currentWeapon._totalBulletCount;
        currentWeaponName = currentWeapon._weaponName;

        foreach (var item in weapons)
        {
            if(!currentWeapons.Contains(item.GetComponent<FirearmController>()._weaponName))
            {
                currentWeapons.Add(item.GetComponent<FirearmController>()._weaponName);
            }
        }

        // if(weapons.Count != currentWeapons.Count)
        // {
        //     currentWeapons.RemoveAll(weapon => !weapons.Any(w => w.GetComponent<FirearmController>()._weaponName == weapon));
        // }

        if(Input.GetKeyDown(KeyCode.K))
        {
            ActivateKillstreak();
        }

    }
    void weaponChange(int index, bool newWeapon = false)
    {
        if(index > weapons.Count - 1)
        {
            Debug.LogWarning("player tried to access a weapon slot where there was no weapon");
            return;
        }
        if(selectedWeapon == index && !newWeapon)
        {
            return;
        }
        
        weapons[selectedWeapon].SetActive(false);
        weapons[index].SetActive(true);
        weapons[selectedWeapon].GetComponent<FirearmController>().inHand = false;
        weapons[index].GetComponent<FirearmController>().inHand = true;
        
        selectedWeapon = index;
    }

    void WeaponReplace(GameObject newWeapon)
    {
        Debug.Log("weapons were full, going to replace");
        GameObject temp = weapons[selectedWeapon];
        weapons.RemoveAt(selectedWeapon);
        Destroy(temp);
        weapons.Insert(selectedWeapon, newWeapon);
        
        weaponChange(selectedWeapon, true);
        currentWeapons.Clear();
        foreach (var item in weapons)
        {
            currentWeapons.Add(item.GetComponent<FirearmController>()._weaponName);
        }
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
        
        if(newWeapon == null)
        return;
        GameObject spawnedWeapon = Instantiate(newWeapon, weaponSpawnPoint);
        spawnedWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        spawnedWeapon.transform.SetPositionAndRotation(weaponSpawnPoint.position, weaponSpawnPoint.rotation);

        if(weapons.Count + 1 == weapons.Capacity)
        {
            WeaponReplace(spawnedWeapon);
        }
        else
        {
        
        weapons.Add(spawnedWeapon);
        weaponChange(weapons.Count - 1);
        }
        NotifyWeaponChange();
    //     if(nearbyPurchasePoint.purchasePointType == PurchasePoint.PurchasePointType.MysteryBox)
    //     {
    //         MysteryBox box = (MysteryBox)nearbyPurchasePoint;
    //     box.HandlePlayerWeaponsUpdated();
    //    }

        
    }

}