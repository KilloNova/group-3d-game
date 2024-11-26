using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : PurchasePoint
{

    private void OnEnable()
    {
    //    FindObjectOfType<PlayerWeaponController>().OnPlayerWeaponsUpdated += HandlePlayerWeaponsUpdated;
    }

    private void OnDisable()
    {
       // FindObjectOfType<PlayerWeaponController>().OnPlayerWeaponsUpdated -= HandlePlayerWeaponsUpdated;
    }

    [SerializeField]
    private bool purchaseLockout;

    float purchaseLockoutDelay = 3f;

    public void HandlePlayerWeaponsUpdated()
    {
        LoadPrefabsFromFolder(folderName);
        List<GameObject> playerWeapons = FindObjectOfType<PlayerWeaponController>().weapons;
        
        // Iterate backwards through the weapons list
        for (int i = weapons.Count - 1; i >= 0; i--)
        {
            for (int j = 0; j < playerWeapons.Count; j++)
            {
                if (weapons[i].GetComponent<FirearmController>()._weaponName.Equals(playerWeapons[j].GetComponent<FirearmController>()._weaponName))
                {
                    weapons.RemoveAt(i);
                    break; // Exit the inner loop since the weapon has been removed
                }
            }
        }
    }

    public void erase()
    {
        List<GameObject> playerWeapons = FindObjectOfType<PlayerWeaponController>().weapons;
        foreach (GameObject weapon in playerWeapons)
        {
            weapons.Remove(weapon);
        }
    }

    string folderName = "Prefabs/Weapons/Firearms";
    public List<GameObject> weapons;

    // Start is called before the first frame update
    void Start()
    {
        HandlePlayerWeaponsUpdated();
        purchasePointType = PurchasePointType.MysteryBox;
    }

    private GameObject GetRandomWeapon()
    {
        return weapons[Random.Range(0, weapons.Count)];
    }




    void LoadPrefabsFromFolder(string path)
    {
        Object[] prefabs = Resources.LoadAll(path);
        foreach (Object prefab in prefabs)
        {
            weapons.Add((GameObject)prefab);
        }
    }

    IEnumerator delayPurchase(float time)
    {
        yield return new WaitForSeconds(time);
        purchaseLockout = false;
    }

    public override GameObject BuyWeapon()
    {
        if(purchaseLockout)
        return null;
        purchaseLockout = true;
        StartCoroutine(delayPurchase(purchaseLockoutDelay));

        buySound.Play();
        GameObject weapon = GetRandomWeapon();
        weapons.Remove(weapon);
        return weapon;
    }
}
