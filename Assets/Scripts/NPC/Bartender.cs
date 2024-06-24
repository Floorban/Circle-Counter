using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bartender : NPCController, IInteractable
{
    [SerializeField] string prompt;
    [SerializeField] GameObject shopPanel;
    public bool inShop;
    RevolverController revolver;
    DialogueManager dialogueManager;
    public GameObject chamberTextbox;
    [SerializeField] DrinkShop shop;
    public bool canSpawn;

    protected override void Start()
    {
        base.Start();
        revolver = FindObjectOfType<RevolverController>();
        dialogueManager = GetComponent<DialogueManager>(); 
        shop = GetComponent<DrinkShop>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void OnInteract()
    {
        if (!revolver.canInteract) return;

        if (!dialogueManager.hasTalked)
        {
            dialogueManager.StartDialogue();
        }
        else
        {
            if (inShop)
            {
                revolver.OnAmmoShopPause(inShop, shopPanel);
                inShop = false;
                chamberTextbox.SetActive(true);
            }
            else
            {
                revolver.OnAmmoShopPause(inShop, shopPanel);
                inShop = true;
                FindObjectOfType<SoundManager>().PlayAmbient("TraderVoice1");
                if (canSpawn)
                {
                    shop.InstantiateRandomDrinks();
                    canSpawn = false;
                }
            }
        }
    }
    public string GetPrompt()
    {
        return prompt;
    }
}
