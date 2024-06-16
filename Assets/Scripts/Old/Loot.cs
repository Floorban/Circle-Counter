using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loot : MonoBehaviour
{
    Button button;
    GunController gunController;
    AmmoInventory ammoInventory;

    [Range(0, 100)]
    public int dropChance;
    [SerializeField] float amount;
    public int upgradeID;
    void Start()
    {
        gunController = FindObjectOfType<GunController>();
        ammoInventory = FindObjectOfType<AmmoInventory>();
        button = GetComponent<Button>();
        button.onClick.AddListener(Clicked);
    }

    public void Clicked()
    {
        switch (upgradeID)
        {
            case 0:
                gunController.ModifyShootChance(amount);
                break;
            case 1:
                gunController.ModifyShootChance(amount);
                break;
            case 2:
                gunController.ModifyShootChance(amount);
                break;
            case 3:
                gunController.ModifyShootChance(amount);
                break;
            case 4:
                gunController.ModifyShootChance(amount);
                break;
            case 5:
                gunController.ModifyShootChance(amount);
                break;
            case 6:
                gunController.ModifyShootChance(amount);
                break;
            case 7:
                gunController.ModifyShootChance(amount);
                break;
        }

        this.gameObject.SetActive(false);
    }
}
