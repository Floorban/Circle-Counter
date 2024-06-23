using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkItem : MonoBehaviour
{
    public Drink drink;

    public Image icon;
    public Text drinkNameText;

    private void Start()
    {
        if (drink != null)
        {
            icon.sprite = drink.icon;
            drinkNameText.text = drink.drinkName;
        }
    }

    public void UseDrink(Player player)
    {
        drink.Use(player);
        Destroy(gameObject); 
    }
}
