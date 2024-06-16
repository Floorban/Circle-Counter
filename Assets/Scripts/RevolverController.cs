using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RevolverController : MonoBehaviour
{
    [SerializeField] GameObject chamberPanel;
    [SerializeField] GameObject inventoryPanel;
    Chamber chamber;
    Inventory inventory;
    Player player;

    public List<Hole> currentHoles;
    public int bulletNum;
    [SerializeField] TextMeshProUGUI bulletNumText;
    void Start()
    {
        chamber = chamberPanel.GetComponentInChildren<Chamber>();
        inventory = inventoryPanel.GetComponentInChildren<Inventory>();
        player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        bulletNumText.text = $"Bullets In Chamber: {bulletNum}";
    }
    public void RandomizeChamber()
    {
        int selectedSlotIndex = Random.Range(0, chamber.holes.Count);
        List<Hole> originalHoles = new List<Hole>(chamber.holes);
        currentHoles.Clear();
        currentHoles.Add(originalHoles[selectedSlotIndex]);

        // Add the remaining slots in clockwise order
        for (int i = 1; i < originalHoles.Count; i++)
        {
            int nextIndex = (selectedSlotIndex + i) % originalHoles.Count;
            currentHoles.Add(originalHoles[nextIndex]);
        }
    }
    public void TryShoot()
    {
        if (currentHoles.Count <= 0) return;

        Bullet currentBullet = currentHoles[0].myBullet;

        if (currentBullet != null)
        {
            bulletNum--;

            if (currentBullet.isReal)
            {
                float randomValue = Random.value;

                if (player.tension < randomValue)
                {
                    RoundManager.instance.player.TakeDamage(currentBullet.dmg);
                    Destroy(currentBullet.gameObject);
                    inventory.ownedBullets.Remove(currentBullet);
                }
                else
                {
                    player.gold -= 5;
                    Debug.Log("bullet in the celling");
                }
            }
            else
            {
                Debug.Log("fake bullet");
            }
        }
        else
        {
            Debug.Log("no bullet");
        }

        currentHoles.RemoveAt(0);

        if (bulletNum > 0)
        {
            player.tension += 0.1f * bulletNum;
            RoundManager.instance.UpdateGold();
        }
    }
    public void OpenChamber()
    {
        bulletNum = 0;
        chamberPanel.SetActive(true);
        inventoryPanel.SetActive(true);
    }
    public void CloseChamber()
    {
        chamberPanel.SetActive(false);
        inventoryPanel.SetActive(false);

        RandomizeChamber();
    }
}
