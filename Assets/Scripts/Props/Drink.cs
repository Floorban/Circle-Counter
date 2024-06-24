using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDrinkEffect
{
    void ApplyEffect(Drink drink);
}
public abstract class Drink : ScriptableObject
{
    public string drinkName;
    public int drinkValue;
    public Sprite icon;
    public string description;
    public enum TargetType
    {
        Player,
        PlayerController,
        RevolverController,
        Inventory,
    }

    public TargetType targetType;
    public virtual void ShowFunction(string drinkDescription) { }
    public virtual void ApplyTo(IDrinkEffect target) { }
}
