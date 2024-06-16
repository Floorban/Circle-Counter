using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clip : Interactable
{
    private void Start()
    {
        gun = FindObjectOfType<GunController>();
    }
    public override void OnMouseDown()
    {
        base.OnMouseDown();
        gun.TryReload(0);
    }
}
