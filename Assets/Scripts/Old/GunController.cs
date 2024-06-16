using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GunController : MonoBehaviour
{
    public Animator animator;

    [Header("Bullets Collection")]
    public AmmoInventory ammoInventory;
    public List<Bullet> bulletsPool;
    public List<Bullet> currentBulletsList;

    [Header("Gun Attribute")]
    [Range(0, 1)] public float shootChance;
    public int addedDmg;
    int moves;

    [Header ("UI Stuff")]
    [SerializeField] TextMeshProUGUI dmgText;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI ammoInChamberText;
    [SerializeField] TextMeshProUGUI shootChanceText;

    void Start()
    {
        ammoInventory = FindObjectOfType<AmmoInventory>();
        animator = GetComponentInChildren<Animator>();
    }
    public void ModifyShootChance(float modifierChange)
    {
        shootChance += modifierChange;
    }
    public void InitializeGun()
    {
        animator.SetBool("isFinished", false);
        currentBulletsList.Clear();

        if (ammoInventory != null)
        {
            bulletsPool = ammoInventory.ownedBullets;
        }

        for (int i = 0; i < bulletsPool.Count; i++)
        {
            bulletsPool[i].isUsed = false;
        }

        ammoText.text = $"Ammo Left:  {bulletsPool.Count}";
        ammoInChamberText.text = $"Ammo In Chamber:  {currentBulletsList.Count}";
        shootChanceText.text = $"Chance to Shoot:  {shootChance * 100f} %";
        //UpdateMoves(0);
    }
    public void TryReload(int addedBulletDmg)
    {
        if (currentBulletsList.Count != 0) return;

        ShuffleDeck();
        List<Bullet> drawnBullets = DrawBullets(5);

        if (ammoInventory != null)
        {
            foreach (Bullet bullet in drawnBullets)
            {
                GameObject bulletGameObject = bullet.gameObject;
                if (bulletGameObject != null)
                {
                    Actions.OnReload(bullet);
                    bulletGameObject.SetActive(false);
                }
            }
        }

        currentBulletsList = drawnBullets;

        animator.SetTrigger("Reload");
        ammoText.text = $"Ammo Left:  {bulletsPool.Count}";
        ammoInChamberText.text = $"Ammo In Chamber:  {currentBulletsList.Count}";
        //UpdateMoves(1);
    }
    void ShuffleDeck()
    {
        // Fisher-Yates shuffle algorithm to shuffle the deck
        for (int i = 0; i < bulletsPool.Count; i++)
        {
            Bullet temp = bulletsPool[i];
            int randomIndex = Random.Range(i, bulletsPool.Count);
            bulletsPool[i] = bulletsPool[randomIndex];
            bulletsPool[randomIndex] = temp;
        }
    }
    List<Bullet> DrawBullets(int count)
    {
        List<Bullet> drawnBullets = new List<Bullet>();

        // Draw bullets from the deck
        for (int i = 0; i < count && i < bulletsPool.Count; i++)
        {
            drawnBullets.Add(bulletsPool[i]);
        }

        // Remove the drawn bullets from the deck
        bulletsPool.RemoveRange(0, Mathf.Min(count, bulletsPool.Count));

        return drawnBullets;
    }
    public void TryShoot()
    {
        if (currentBulletsList.Count == 0 || currentBulletsList[0].isUsed) return;
        float randomValue = Random.value;

        if (randomValue <= shootChance)
        {
            if (currentBulletsList[0].isReal)
            {
                animator.SetTrigger("Fire");
                GameManager.instance.currentOpponent.TakeDamage(currentBulletsList[0].dmg + addedDmg);
                dmgText.text = $"Damage Caused:  {currentBulletsList[0].dmg + addedDmg}";
            }
            else
            {
                animator.SetTrigger("Fail");
                dmgText.text = $"It's a fake bullet.";
            }

            currentBulletsList[0].isUsed = true;
        }
        else
        {
            // Player misses the shot
            // animator.SetTrigger("Miss");
            dmgText.text = $"You missed the shot.";
        }

        ammoInChamberText.text = $"Ammo In Chamber:  {currentBulletsList.Count}";
        //UpdateMoves(1);
    }
    public void TryEmpty()
    {
        if (currentBulletsList.Count == 0) return;
        animator.SetTrigger("Empty");
        Destroy(currentBulletsList[0].gameObject);
        currentBulletsList.RemoveAt(0);
        ammoInChamberText.text = $"Ammo In Chamber:  {currentBulletsList.Count}";
        //UpdateMoves(1);
    }
    /*public void TrySkip()
    {
        if (currentBulletsList.Count == 0 || moves <= 0) return;
        if (currentBulletsList[0].isReal)
        {
            Debug.Log("You lose a real bullet.");
        }
        else
        {
            Debug.Log("It's a fake bullet.");
        }
        animator.SetTrigger("Empty");
        currentBulletsList.RemoveAt(0);
        ammoInChamberText.text = $"Ammo In Chamber:  {currentBulletsList.Count}";
        UpdateMoves(1);
    }*/ // Do I need to inform player which bullet they abandoned?
   public void UpdateMoves(int numOfMoves)
    {
        moves += numOfMoves;
        shootChance -= moves * 0.05f;
    }
}
