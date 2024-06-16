using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Attributes")]
    public int hp;
    public int maxHp;
    public bool isDead;
    public int gold;
    [Range (0, 1f)] public float tension;

    [Header("UI")]
    [SerializeField] Image tensionBar;
    [SerializeField] TextMeshProUGUI tensionText;

    void Start()
    {
        InitializeStatus();
    }
    private void Update()
    {
        tensionBar.fillAmount = tension / 1f;
        tensionText.text = $"Tension {tension}";
    }
    public void InitializeStatus()
    {
        hp = maxHp;
        isDead = false;
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        Actions.OnDamageCaused.Invoke();

        if (hp <= 0)
        {
            isDead = true;
            Actions.OnLevelFinished.Invoke();
        }
    }
}
