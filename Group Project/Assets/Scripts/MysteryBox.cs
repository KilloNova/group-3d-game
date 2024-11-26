using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : PurchasePoint
{

    private void OnEnable()
    {
        FindObjectOfType<PlayerWeaponController>().OnPlayerWeaponsUpdated += HandlePlayerWeaponsUpdated;
    }

    private void OnDisable()
    {
        FindObjectOfType<PlayerWeaponController>().OnPlayerWeaponsUpdated -= HandlePlayerWeaponsUpdated;
    }

    private void HandlePlayerWeaponsUpdated()
    {
        List<GameObject> playerWeapons = FindObjectOfType<PlayerWeaponController>().weapons;
        foreach (GameObject weapon in playerWeapons)
        {
            weapons.Remove(weapon);
        }
    }

    string folderName = "Prefabs/Weapons/Firearms";
    public List<GameObject> weapons;


    public int cost;
    // Start is called before the first frame update
    void Start()
    {
        LoadPrefabsFromFolder(folderName);
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



    // Update is called once per frame
    void Update()
    {
        
    }

    public override GameObject BuyWeapon()
    {
        buySound.Play();
        GameObject weapon = GetRandomWeapon();
        weapons.Remove(weapon);
        return weapon;
    }
}
