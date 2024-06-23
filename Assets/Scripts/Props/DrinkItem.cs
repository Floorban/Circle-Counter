using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrinkItem : MonoBehaviour
{
    public Drink drink;
    public Image icon;
    public int cost;
    //public TextMeshProUGUI drinkNameText;
    public TextMeshProUGUI attributeText;
    public bool oneTimeUse;

    private void Start()
    {
        icon = GetComponent<Image>();

        if (drink != null)
        {
            icon.sprite = drink.icon;
            cost = drink.drinkValue;
            //drinkNameText.text = drink.drinkName;
            attributeText.text = $"{drink.description} \n price: {cost}";
        }
    }

    public void UseDrink()
    {
        IDrinkEffect target = GetTargetBasedOnType(drink.targetType);
        if (target != null)
        {
            FindObjectOfType<Player>().gold -= cost;
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

        if (oneTimeUse) 
        Destroy(gameObject);
    }
}
