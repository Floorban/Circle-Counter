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
    public Sprite icon;

    public virtual void ApplyTo(IDrinkEffect target)
    {
        // Base apply effect, can be overridden by specific drinks
    }
}
[CreateAssetMenu(fileName = "NewHealthDrink", menuName = "Drink/HealthDrink")]
public class HealthDrink : Drink
{
    public int healthBoost;

    public override void ApplyTo(IDrinkEffect target)
    {
        target.ApplyEffect(this);
    }
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