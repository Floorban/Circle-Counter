using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMaxHealthDrink", menuName = "Drink/MaxHealthDrink")]
public class MaxHpDrink : Drink
{
    public int healthBoost;

    private void OnEnable()
    {
        description = $"Increases max health by {healthBoost}.";
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
