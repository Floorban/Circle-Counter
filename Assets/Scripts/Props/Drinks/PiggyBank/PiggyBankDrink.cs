using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBankDrink", menuName = "Drink/BankDrink")]
public class PiggyBankDrink : Drink
{
    public int goldBoost;

    private void OnEnable()
    {
        description = $"Gain ${goldBoost} every time when you finish a circle.";
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
