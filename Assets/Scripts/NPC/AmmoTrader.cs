using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTrader : NPCController, IInteractable
{
    [SerializeField] string prompt;
    [SerializeField] GameObject shopPanel;
    bool inShop;
    protected override void Start()
    {
        base.Start(); 
    }

    protected override void Update()
    {
        base.Update(); 

        if (inShop)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                shopPanel.SetActive(false);
                inShop = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    
    public void OnInteract()
    {
        shopPanel.SetActive(true);
        inShop = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public string GetPrompt()
    {
        return prompt;
    }
}
