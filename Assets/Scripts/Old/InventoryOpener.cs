using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpener : MonoBehaviour
{
    [SerializeField] GameObject inventory;
/*    private void Start()
    {
        inventory.SetActive(false);
    }*/
    public void OpenPanel()
    {
        inventory.SetActive(true);
    }
    public void ClosePanel()
    {
        inventory.SetActive(false);
    }
}
