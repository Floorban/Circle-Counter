using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnergyDrink", menuName = "Drink/EnergyDrink")]
public class EnergyDrink : Drink
{
    public int energyBoost;

    public override void Use(Player player)
    {
        player.energy += energyBoost;
    }
}
