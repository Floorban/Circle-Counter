using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoInventory : MonoBehaviour
{
    public List<Bullet> ownedBullets;
    public List<GameObject> bulletPrefabs;
    [SerializeField] Transform spawnTrans;
    [SerializeField] Sprite r_bulletSprite;

    public void GetNewBullet()
    {
        if (bulletPrefabs.Count > 0)
        {
            GameObject bulletPrefab = bulletPrefabs[Random.Range(0, bulletPrefabs.Count)];
            GameObject newBulletObject = Instantiate(bulletPrefab, spawnTrans);

            Bullet newBullet = newBulletObject.GetComponent<Bullet>();
            ownedBullets.Add(newBullet);
        }
        else
        {
            Debug.LogWarning("No bullet prefabs available to instantiate.");
        }
    }
    public void UpgradeBullet(int amount)
    {
        Bullet bulletToUpgrade = GetRandomBullet();

        if (bulletToUpgrade != null)
        {
            bulletToUpgrade.dmg += amount;
        }
        else
        {
            Debug.Log("No bullet found to upgrade.");
        }
    }
    public void ConvertBullet()
    {
        Bullet bulletToConvert = GetFakeBullet();

        if (bulletToConvert != null)
        {
            bulletToConvert.isReal = true;
            bulletToConvert.image.sprite = r_bulletSprite;
        }
        else
        {
            Debug.Log("No bullet found to upgrade.");
        }
    }
    public void AbandonBullet()
    {
        Bullet bulletToAbandon = GetRandomBullet();

        if (bulletToAbandon != null)
        {
            ownedBullets.Remove(bulletToAbandon);
            Destroy(bulletToAbandon.gameObject);
        }
        else
        {
            Debug.Log("No bullet found to abandon.");
        }
    }
    Bullet GetRandomBullet()
    {
        if (ownedBullets.Count > 0)
        {
            int randomIndex = Random.Range(0, ownedBullets.Count);
            return ownedBullets[randomIndex];
        }
        else
        {
            return null;
        }
    }
    Bullet GetFakeBullet()
    {
        List<Bullet> fakeBullets = ownedBullets.FindAll(bullet => !bullet.isReal);

        if (fakeBullets.Count > 0)
        {
            int randomIndex = Random.Range(0, fakeBullets.Count);
            return fakeBullets[randomIndex];
        }
        else
        {
            return null;
        }
    }
}
