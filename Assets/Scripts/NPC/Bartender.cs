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

    protected override void Start()
    {
        base.Start();
        revolver = FindObjectOfType<RevolverController>();
        dialogueManager = GetComponent<DialogueManager>(); 
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
                revolver.OnShopPause(inShop, shopPanel);
                inShop = false;
            }
            else
            {
                revolver.OnShopPause(inShop, shopPanel);
                inShop = true;
            }
        }
    }
    public string GetPrompt()
    {
        return prompt;
    }
}
