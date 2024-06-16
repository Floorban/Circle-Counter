using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hole : MonoBehaviour
{
    RevolverController gun;

    public bool isFull;
    public Button button;
    public Image image;
    public Bullet myBullet;

    [SerializeField] TextMeshProUGUI bulletAttributeText;
    private void Start()
    {
        gun = FindObjectOfType<RevolverController>();
        button = this.GetComponent<Button>();
        image = this.GetComponent<Image>();
        button.enabled = false;
    }
    private void Update()
    {
        if (myBullet != null)
            bulletAttributeText.text = $"Damage: {myBullet.dmg} \nReward: {myBullet.reward}";
        else
            bulletAttributeText.text = $"Empty Slot";
    }
    public void LoadBullet()
    {
        isFull = true;
        button.enabled = false;
        image.color = Color.yellow;
        Actions.OnHoleSelected(this);
        //FindObjectOfType<Player>().UpdateUI();
        gun.bulletNum++;    
    }
    public void ResetHole()
    {
        isFull = false;
        image.color = Color.white;
    }
}
