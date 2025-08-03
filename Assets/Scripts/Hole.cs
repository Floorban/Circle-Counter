using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hole : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RevolverController gun;
    public Sprite hole_sprite;
    public bool isFull;
    public Button button;
    public Image image;
    public Bullet myBullet;

    public TextMeshProUGUI bulletAttributeText;
    private void Start()
    {
        gun = FindObjectOfType<RevolverController>();
        button = this.GetComponent<Button>();
        image = this.GetComponent<Image>();
        button.enabled = false;
        hole_sprite = image.sprite;
    }

    public void LoadBullet()
    {
        if (isFull) return;

        isFull = true;
        button.enabled = false;
        image.color = Color.yellow;
        Actions.OnHoleSelected(this);
        FindObjectOfType<SoundManager>().PlaySound("LoadBullet", 1);
        gun.bulletNum++;
        image.sprite = myBullet.image.sprite;
    }
    public void ResetHole()
    {
        isFull = false;
        image.color = Color.white;
        image.sprite = hole_sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (myBullet != null)
        {
            bulletAttributeText.text = $"Damage: {myBullet.dmg} \nReward: {myBullet.reward}";
            if (myBullet.effect != null)
                bulletAttributeText.text = $"Damage: {myBullet.dmg} \nReward: {myBullet.reward} \n {myBullet.effect.name}";
        }
        else
        {
            bulletAttributeText.text = $"Empty Slot";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        bulletAttributeText.text = string.Empty;
    }
}
