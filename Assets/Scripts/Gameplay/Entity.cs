using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Attributes")]
    public int hp;
    public int maxHp;
    public bool isDead;
    void Start()
    {
        InitializeStatus();
    }
    public void InitializeStatus()
    {
        hp = maxHp;
        isDead = false;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
}
