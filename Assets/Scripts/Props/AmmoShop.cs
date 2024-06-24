using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoShop : MonoBehaviour
{
    public Projectile[] availableProjectiles;
    public GameObject bulletPrefab;
    public Transform ammoShopContainer;
    public int maxAmmoNum;

    private void OnEnable()
    {
        Actions.OnLevelEnd += InstantiateRandomBullets;
    }
    private void OnDisable()
    {
        Actions.OnLevelEnd -= InstantiateRandomBullets;
    }
    public void InstantiateRandomBullets()
    {
        // Clear existing drink items
        foreach (Transform child in ammoShopContainer)
        {
            Destroy(child.gameObject);
        }

        List<Projectile> selectedAmmo = new List<Projectile>();
        while (selectedAmmo.Count < maxAmmoNum)
        {
            Projectile randomAmmo = availableProjectiles[Random.Range(0, availableProjectiles.Length)];

            selectedAmmo.Add(randomAmmo);

            GameObject drinkItem = Instantiate(bulletPrefab, ammoShopContainer);
            Bullet drinkItemScript = drinkItem.GetComponent<Bullet>();
            drinkItemScript.projectile = randomAmmo;
            drinkItemScript.inShop = true;

        }
    }
}
