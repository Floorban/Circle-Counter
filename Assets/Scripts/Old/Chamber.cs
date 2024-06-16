using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chamber : MonoBehaviour
{
    public List<Hole> holes;
    Inventory inventory;

    void Awake()
    {
        foreach (Transform child in transform)
        {
            Hole hole = child.GetComponent<Hole>();
            if (hole != null)
            {
                holes.Add(hole);
            }
        }
    }
    private void OnEnable()
    {
        Actions.OnHoleSelected += DisableHoles;
        Actions.OnBulletDeselected += DisableButtons;
    }
    private void OnDisable()
    {
        Actions.OnHoleSelected -= DisableHoles;
        Actions.OnBulletDeselected -= DisableButtons;
    }
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        InitializeChamber();
    }

    void InitializeChamber()
    {
        for (int i = 0; i < holes.Count; i++)
        {
            holes[i].isFull = false;
        }
    }
/*    public void RandomizeChamber()
    {
        int selectedSlotIndex = Random.Range(0, holes.Count);
        List<Hole> originalHoles = new List<Hole>(holes);
        holes.Clear();
        holes.Add(originalHoles[selectedSlotIndex]);

        // Add the remaining slots in clockwise order
        for (int i = 1; i < originalHoles.Count; i++)
        {
            int nextIndex = (selectedSlotIndex + i) % originalHoles.Count;
            holes.Add(originalHoles[nextIndex]);
        }
    }*/

    public void ResetChamber()
    {
        for (int i = 0; i < holes.Count; i++)
        {
            holes[i].ResetHole();
            if (holes[i].myBullet != null)
            {
                inventory.ownedBullets.Add(holes[i].myBullet);
                holes[i].myBullet.gameObject.SetActive(true);
                holes[i].myBullet = null;
            }
        }

        for (int i = 0;i < inventory.ownedBullets.Count ;i++) 
        {
            inventory.ownedBullets[i].ResetBullet();
        }

        //FindObjectOfType<Player>().reward = 0;
    }

    public void EnableHoles()
    {
        for (int i = 0; i < holes.Count; i++)
        {
            holes[i].button.enabled = true;
        }
    }
    public void DisableHoles(Hole hole)
    {
        for (int i = 0; i < holes.Count; i++)
        {
            holes[i].button.enabled = false;
        }

        hole.myBullet = inventory.selectedBullet;
        FindObjectOfType<Player>().reward += inventory.selectedBullet.reward;
        inventory.selectedBullet.gameObject.SetActive(false);
        inventory.ownedBullets.Remove(inventory.selectedBullet);
        inventory.EnableButtons();
    }

    void DisableButtons()
    {
        for (int i = 0; i < holes.Count; i++)
        {
            holes[i].button.enabled = false;
        }
    }
}
