using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LootPanel : MonoBehaviour
{
    [SerializeField] List<Loot> lootList = new List<Loot>();
    List<GameObject> spawnedLootObjects = new List<GameObject>();

    [SerializeField] GameObject spawnPanel;
    
    public void OpenPanel()
    {
        Time.timeScale = 0f;
        InstantiateButtons(spawnPanel.transform, 3);
    }
    public void ClosePanel(int buttonID)
    {
        Time.timeScale = 1f;
        foreach (GameObject lootObject in spawnedLootObjects)
        {
            Destroy(lootObject);
        }
        spawnedLootObjects.Clear();
        this.gameObject.SetActive(false);
    }
    Loot GetRandomUpgrade(List<Loot> excludeList)
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance && !excludeList.Contains(item))
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0)
        {
            Loot droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];
            return droppedItem;
        }
        return null;
    }

    void InstantiateButtons(Transform parentTransform, int numberOfButtons)
    {
        List<Loot> excludeList = new List<Loot>();
        for (int i = 0; i < numberOfButtons; i++)
        {
            Loot droppedItem = GetRandomUpgrade(excludeList);
            if (droppedItem != null)
            {
                GameObject lootGameObject = Instantiate(droppedItem.gameObject, parentTransform);
                lootGameObject.transform.parent = spawnPanel.transform;
                spawnedLootObjects.Add(lootGameObject);
                excludeList.Add(droppedItem);
            }
        }
    }
}
