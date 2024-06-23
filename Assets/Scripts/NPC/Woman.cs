using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Woman : NPCController, IInteractable
{
    [SerializeField] string prompt;
    [SerializeField] GameObject shopPanel;
    public bool inShop;
    RevolverController revolver;
    protected override void Start()
    {
        base.Start();
        revolver = FindObjectOfType<RevolverController>();
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
        }
    }
    public string GetPrompt()
    {
        return prompt;
    }
}
