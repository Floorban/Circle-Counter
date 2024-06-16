using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : Interactable
{
    private void Start()
    {
        gun = GetComponentInParent<GunController>();
    }
    public override void OnMouseDown()
    {
        base.OnMouseDown();
        gun.TryShoot();
    }
}
