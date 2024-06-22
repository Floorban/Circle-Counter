using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTrader : NPCController, IInteractable
{
    [SerializeField] string prompt;
    [SerializeField] GameObject shopPanel;
    public bool inShop;
    protected override void Start()
    {
        base.Start();
        //inShop = true;
        //shopPanel.SetActive(true);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void OnInteract()
    {
        if (inShop)
        {
            FindObjectOfType<RevolverController>().OnShopPause(inShop, shopPanel);
            inShop = false;
        }
        else
        {
            FindObjectOfType<RevolverController>().OnShopPause(inShop, shopPanel);
            inShop = true;
            FindObjectOfType<SoundManager>().PlayAmbient("TraderVoice1");
        }
    }
    public string GetPrompt()
    {
        return prompt;
    }
}
