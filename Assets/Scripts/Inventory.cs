using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
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
