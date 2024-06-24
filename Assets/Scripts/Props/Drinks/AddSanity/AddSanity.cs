using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAddSanityDrink", menuName = "Drink/AddSanityDrink")]
public class AddSanity : Drink
{
    public int sanityBoost;

    private void OnEnable()
    {
        //description = $"Recover sanity.";
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
