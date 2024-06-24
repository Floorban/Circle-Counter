using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeedDrink", menuName = "Drink/SpeedDrink")]
public class SpeedDrink : Drink
{
    public float speedBoost;

    private void OnEnable()
    {
        //description = $"sanity losing speed would be a bit slower.";
        targetType = TargetType.PlayerController;
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
