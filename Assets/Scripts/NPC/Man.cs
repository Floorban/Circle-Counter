using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : NPCController, IInteractable
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
            revolver.OnShopPause(inShop, shopPanel);
            inShop = false;
        }
        else
        {
            revolver.OnShopPause(inShop, shopPanel);
            inShop = true;
        }
    }
    public string GetPrompt()
    {
        return prompt;
    }
}
