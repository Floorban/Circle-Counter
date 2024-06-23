using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrinkItem : MonoBehaviour
{
    public Drink drink;
    public Image icon;
    //public TextMeshProUGUI drinkNameText;
    public TextMeshProUGUI attributeText;

    private void Start()
    {
        icon = GetComponent<Image>();

        if (drink != null)
        {
            icon.sprite = drink.icon;
            //drinkNameText.text = drink.drinkName;
            attributeText.text = drink.description;
        }
    }

    public void UseDrink()
    {
        IDrinkEffect target = GetTargetBasedOnType(drink.targetType);
        if (target != null)
        {
            UseDrink(target);
        }
    }

    private IDrinkEffect GetTargetBasedOnType(Drink.TargetType targetType)
    {
        switch (targetType)
        {
            case Drink.TargetType.Player:
                return FindObjectOfType<Player>(); 

            case Drink.TargetType.PlayerController:
                return FindObjectOfType<PlayerController>(); 


            default:
                return null;
        }
    }

    public void UseDrink(IDrinkEffect target)
    {
        drink.ApplyTo(target);
        Destroy(gameObject);
    }
}
