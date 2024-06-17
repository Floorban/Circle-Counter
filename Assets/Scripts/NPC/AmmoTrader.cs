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
        inShop = true;
        shopPanel.SetActive(true);
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
        Debug.Log("test 1");
        shopPanel.SetActive(true);
        inShop = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("test 2");
    }
    public string GetPrompt()
    {
        return prompt;
    }
}
