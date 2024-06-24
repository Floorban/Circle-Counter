using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTrader : NPCController, IInteractable
{
    [SerializeField] string prompt;
    [SerializeField] GameObject shopPanel;
    public bool inShop;
    RevolverController revolver;
    [SerializeField] AmmoShop shop;
    public bool canSpawn;
    protected override void Start()
    {
        base.Start();
        revolver = FindObjectOfType<RevolverController>();
        shop = GetComponent<AmmoShop>();
        //inShop = true;
        //shopPanel.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void OnInteract()
    {
        if (!revolver.canInteract) return;

        if (inShop)
        {
            revolver.OnAmmoShopPause(inShop, shopPanel);
            inShop = false;
        }
        else
        {
            revolver.OnAmmoShopPause(inShop, shopPanel);
            inShop = true;
            FindObjectOfType<SoundManager>().PlayAmbient("TraderVoice2");
            if (canSpawn)
            {
                shop.InstantiateRandomBullets();
                canSpawn = false;
            }
        }
    }
    public string GetPrompt()
    {
        return prompt;
    }
}
