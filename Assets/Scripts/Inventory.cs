using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IDrinkEffect
{
    public List<Bullet> ownedBullets;
    Chamber chamber;
    public Bullet selectedBullet;
    GridLayoutGroup gridLayout;
    void Awake()
    {
        foreach (Transform child in transform)
        {
            Bullet bullet = child.GetComponent<Bullet>();
            if (bullet != null)
            {
                ownedBullets.Add(bullet);
            }
        }
    }
    private void OnEnable()
    {
        Actions.OnBulletSelected += DisableButtons;
        Actions.OnBulletDeselected += EnableButtons;
    }
    private void OnDisable()
    {
        Actions.OnBulletSelected -= DisableButtons;
        Actions.OnBulletDeselected -= EnableButtons;
    }
    void Start()
    {
        chamber = FindObjectOfType<Chamber>();
        gridLayout = GetComponent<GridLayoutGroup>();
    }
    public void ApplyEffect(Drink drink)
    {
        switch (drink)
        {
            case BulletPowerupDrink bulletDrink:
                for (int i = 0; i < bulletDrink.bulletNum; i++)
                {
                    Bullet bulletToUpgrade = GetRandomBullet();
                    if (bulletToUpgrade != null)
                    {
                        bulletToUpgrade.dmg -= bulletDrink.bulletDmgBoost;
                    }
                    else
                    {
                        break;
                    }
                }
                break;
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
    void DisableButtons(Bullet s_bullet)
    {
        for (int i = 0; i < ownedBullets.Count; i++) 
        {
            ownedBullets[i].button.enabled = false;
        }

        s_bullet.button.enabled = true;
        selectedBullet = s_bullet;
        //gridLayout.enabled = false;
        chamber.EnableHoles();
    }

    public void EnableButtons()
    {
        for (int i = 0; i < ownedBullets.Count; i++)
        {
            ownedBullets[i].button.enabled = true;
        }

        selectedBullet = null;
    }

    public void DisableBulletButton()
    {
        for (int i = 0;i < ownedBullets.Count;i++) 
        {
            ownedBullets[i].ResetBullet();
        }
    }

}
