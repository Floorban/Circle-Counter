    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bullet : MonoBehaviour
{
    [Header ("Bullet Attributes")]
    public Projectile projectile;
    public Image image;
    public bool isReal;
    public bool isUsed;
    public bool inShop;
    public int dmg;
    public int reward;
    public int price;
    [Range(0f, 1f)] public float activeChance;
    Inventory inventory;
    Player player;

    [Header("UI")]
    public Button button;
    public bool isSelected;
    [SerializeField] TextMeshProUGUI attributeText;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        player = FindObjectOfType<Player>();

        image = GetComponent<Image>();
        button = GetComponent<Button>();

        image.sprite = projectile.sprite;
        isReal = projectile.isReal;
        isUsed = false;
        dmg = projectile.dmg;
        reward = projectile.reward;
        price = projectile.price;
    }
    private void Update()
    {
        attributeText.text = $"Damage: {dmg} \nReward: {reward} \nPrice: {price}";

        if (dmg <= 0) dmg = 0;
    }
    public void Selected()
    {
        if (!inShop)
        {
            isSelected = !isSelected;

            if (isSelected)
            {
                image.color = Color.red;
                FindObjectOfType<SoundManager>().PlaySound("BulletSelect", 1);
                Actions.OnBulletSelected(this);
            }
            else
            {
                image.color = Color.white;
                Actions.OnBulletDeselected();
            }
        }
        else
        {
            if (player.gold >= price)
            {
                inventory.ownedBullets.Add(this);
                this.gameObject.transform.SetParent(inventory.gameObject.transform);
                player.gold -= price;
                inShop = false;
                Actions.OnBulletDeselected();
                ResetBullet();
                FindObjectOfType<SoundManager>().PlaySound("BuyBullet", 0.6f);
                //RoundManager.instance.UpdateUI();
            }
            else
            {
                Debug.Log("not enough gold");
                FindObjectOfType<SoundManager>().PlayAmbient("TraderVoice1");
            }
        }
    }

    public void ResetBullet()
    {
        isSelected = false;
        image.color = Color.white;
    }
}
