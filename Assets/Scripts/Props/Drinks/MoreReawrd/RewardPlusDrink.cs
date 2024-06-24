using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewReward+Drink", menuName = "Drink/Reward+Drink")]
public class RewardPlusDrink : Drink
{
    public int rewardBoost;

    private void OnEnable()
    {
        description = $"Increases sanity reward from each shot by {rewardBoost}.";
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
