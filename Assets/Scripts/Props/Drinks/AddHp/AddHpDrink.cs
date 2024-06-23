using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAddHealthDrink", menuName = "Drink/AddHealthDrink")]
public class AddHpDrink : Drink
{
    public int healthBoost;

    private void OnEnable()
    {
        description = $"Recover health.";
        targetType = TargetType.Player;
    }

    public override void ShowFunction(string drinkDescription)
    {
        drinkDescription = description;
    }

    public override void ApplyTo(IDrinkEffect target)
    {
        target.ApplyEffect(this);
    }
}
