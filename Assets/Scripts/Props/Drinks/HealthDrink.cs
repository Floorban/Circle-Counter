using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthDrink", menuName = "Drink/HealthDrink")]
public class HealthDrink : Drink
{
    public int healthBoost;

    public override void Use(Player player)
    {
        player.hp += healthBoost;
    }
}