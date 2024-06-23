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
    }

    public TargetType targetType;
    public virtual void ShowFunction(string drinkDescription) { }
    public virtual void ApplyTo(IDrinkEffect target) { }
}

// EnergyDrink
[CreateAssetMenu(fileName = "NewEnergyDrink", menuName = "Drink/EnergyDrink")]
public class EnergyDrink : Drink
{
    public float energyBoost;

    public override void ApplyTo(IDrinkEffect target)
    {
        target.ApplyEffect(this);
    }
}

// DamageDrink
[CreateAssetMenu(fileName = "NewDamageDrink", menuName = "Drink/DamageDrink")]
public class DamageDrink : Drink
{
    public int damageBoost;

    public override void ApplyTo(IDrinkEffect target)
    {
        target.ApplyEffect(this);
    }
}

// AvoidFirstShotDrink
[CreateAssetMenu(fileName = "NewAvoidFirstShotDrink", menuName = "Drink/AvoidFirstShotDrink")]
public class AvoidFirstShotDrink : Drink
{
    public override void ApplyTo(IDrinkEffect target)
    {
        target.ApplyEffect(this);
    }
}