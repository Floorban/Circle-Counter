using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVisionDrink", menuName = "Drink/VisionDrink")]
public class VisionDrink : Drink
{
    public float visionBoost;

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
