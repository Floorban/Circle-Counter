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

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inShop)
            {
                shopPanel.SetActive(false);
                inShop = false;
                FindAnyObjectByType<PlayerCam>().UnlockCam();
                FindObjectOfType<RevolverController>().canControl = true;
            }
            else
            {
                shopPanel.SetActive(true);
                inShop = true;
                FindAnyObjectByType<PlayerCam>().LockCam();
                FindObjectOfType<RevolverController>().canControl = false;
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
