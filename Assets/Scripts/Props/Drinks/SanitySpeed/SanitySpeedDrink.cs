using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSanitySpeedDrink", menuName = "Drink/SanitySpeedDrink")]
public class SanitySpeedDrink : Drink
{
    public float sanitySpeedBoost;

    private void OnEnable()
    {
        //description = $"sanity losing speed would be a bit slower.";
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
