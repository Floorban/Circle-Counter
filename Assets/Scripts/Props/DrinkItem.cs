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
            var player = FindObjectOfType<Player>();

            if (player.gold >= cost)
            {
                player.gold -= cost;
                UseDrink(target);
            }
        }
    }

    private IDrinkEffect GetTargetBasedOnType(Drink.TargetType targetType)
    {
        switch (targetType)
        {
            case Drink.TargetType.Inventory:
                return FindObjectOfType<Inventory>();
            case Drink.TargetType.Player:
                return FindObjectOfType<Player>(); 
            case Drink.TargetType.PlayerController:
                return FindObjectOfType<PlayerController>();
            case Drink.TargetType.RevolverController:
                return FindObjectOfType<RevolverController>();

            default:
                return null;
        }
    }

    public void UseDrink(IDrinkEffect target)
    {
        drink.ApplyTo(target);
        FindObjectOfType<SoundManager>().PlaySound("Drink", 1f);
        if (oneTimeUse) 
        Destroy(gameObject);
    }
}
