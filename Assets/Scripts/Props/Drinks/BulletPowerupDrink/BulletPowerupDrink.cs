using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBulletPowerupDrink", menuName = "Drink/BulletPowerupDrink")]
public class BulletPowerupDrink : Drink
{
    public int bulletDmgBoost;
    public int bulletNum;

    private void OnEnable()
    {
        description = $"Decrease {bulletNum} bullet's damage by {bulletDmgBoost} from your inventory";
        targetType = TargetType.Inventory;
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
