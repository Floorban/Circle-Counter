using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMaxSanityDrink", menuName = "Drink/MaxSanityDrink")]
public class MaxSanityDrink : Drink
{
    public int sanityBoost;

    private void OnEnable()
    {
        description = $"Increases max sanity by {sanityBoost}.";
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
