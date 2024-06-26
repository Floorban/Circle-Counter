using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkShop : MonoBehaviour
{
    public Drink[] availableDrinks; 
    public GameObject drinkItemPrefab; 
    public Transform drinkShopContainer;
    public int spawnNum;
    private void OnEnable()
    {
        Actions.OnLevelEnd += InstantiateRandomDrinks;
    }
    private void OnDisable()
    {
        Actions.OnLevelEnd -= InstantiateRandomDrinks;
    }

    public void InstantiateRandomDrinks()
    {
        // Clear existing drink items
        foreach (Transform child in drinkShopContainer)
        {
            Destroy(child.gameObject);
        }

        // Instantiate 3 random different types of drink
        List<Drink> selectedDrinks = new List<Drink>();
        while (selectedDrinks.Count < spawnNum)
        {
            Drink randomDrink = availableDrinks[Random.Range(0, availableDrinks.Length)];
            if (!selectedDrinks.Contains(randomDrink))
            {
                selectedDrinks.Add(randomDrink);

                GameObject drinkItem = Instantiate(drinkItemPrefab, drinkShopContainer);
                DrinkItem drinkItemScript = drinkItem.GetComponent<DrinkItem>();
                drinkItemScript.drink = randomDrink;
            }
        }
    }
}
