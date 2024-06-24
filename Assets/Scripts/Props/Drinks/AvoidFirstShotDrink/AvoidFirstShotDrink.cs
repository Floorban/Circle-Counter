using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAFSDrink", menuName = "Drink/AFSDrink")]
public class AvoidFirstShotDrink : Drink
{
    public int bulletDmgBoost;

    private void OnEnable()
    {
        description = $"Avoid the first bullet you shoot at your self";
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
