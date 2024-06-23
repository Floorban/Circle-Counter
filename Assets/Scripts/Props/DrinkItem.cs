using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrinkItem : MonoBehaviour
{
    public Drink drink;
    public Image icon;
    public TextMeshProUGUI drinkNameText;
    public TextMeshProUGUI attributeText;

    private void Start()
    {
        if (drink != null)
        {
            icon.sprite = drink.icon;
            drinkNameText.text = drink.drinkName;
        }
    }

    public void UseDrink(IDrinkEffect target)
    {
        drink.ApplyTo(target);
        Destroy(gameObject);
    }
}
