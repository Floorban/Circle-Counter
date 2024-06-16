using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Opponent : MonoBehaviour
{
    [Header("Attributes")]
    public int hp;
    public int maxHp;
    public bool isDead;

    [Header("Events")]
    public UnityEvent levelCompleted;
    public UnityEvent takeDmg;
    private void Start()
    {
        //levelCompleted.AddListener(GameManager.instance.EndLevel);
        //takeDmg.AddListener(GameManager.instance.UpdateUI);
    }
    public void InitializeStatus()
    {
        hp = maxHp;
        isDead = false;
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        takeDmg.Invoke();

        if (hp <= (int)(0.1f * maxHp) && hp > 0)
        {
            levelCompleted.Invoke();
        }
        else if (hp <= 0)
        {
            isDead = true;
            levelCompleted.Invoke();
        }

    }
}
