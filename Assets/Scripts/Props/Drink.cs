using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Drink : ScriptableObject
{
    public string drinkName;
    public Sprite icon;

    public abstract void Use(Player player);
}