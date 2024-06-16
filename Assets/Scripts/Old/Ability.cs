using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ability : MonoBehaviour
{
    private void OnEnable()
    {
        Actions.OnReload += ActivateAbility;
    }
    private void OnDisable()
    {
        Actions.OnReload -= ActivateAbility;
    }
    void Start()
    {
        GunController gun = FindObjectOfType<GunController>();
    }

    public void ActivateAbility(Bullet bullet)
    {
        bullet.dmg++;
    }

    
}
