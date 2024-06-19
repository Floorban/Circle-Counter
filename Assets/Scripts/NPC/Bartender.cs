using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bartender : NPCController, IInteractable
{
    [SerializeField] string prompt;
    [SerializeField] GameObject shopPanel;
    public bool inShop;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void OnInteract()
    {
        if (inShop)
        {
            shopPanel.SetActive(false);
            inShop = false;
            FindAnyObjectByType<PlayerCam>().UnlockCam();
            FindObjectOfType<RevolverController>().CloseChamberP();
        }
        else
        {
            shopPanel.SetActive(true);
            inShop = true;
            FindAnyObjectByType<PlayerCam>().LockCam();
            FindObjectOfType<RevolverController>().OpenChamberP();
            FindObjectOfType<SoundManager>().PlayAmbient("TraderVoice1");
        }
    }
    public string GetPrompt()
    {
        return prompt;
    }
}
