using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Attributes")]
    public int hp;
    public int maxHp;
    public bool isDead;
    public int gold;

    [Header("UI")]
    [SerializeField] Image EnergyBar;
    [SerializeField] TextMeshProUGUI EnergyText;
    [SerializeField] Image hpBar;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] TextMeshProUGUI goldText;

    public bool shootSelf;
    public int reward;
    void EndRound()
    {
        Debug.Log("round ends");
        SceneManager.LoadScene(0);
    }
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
        UpdateUI() ;

        if (hp <= 0)
        {
            isDead = true;
            EndRound();
        }
    }
    public void UpdateGold()
    {
        gold += reward;
        UpdateUI();
    }
    public void UpdateUI()
    {
      /*  hpBar.fillAmount = (float)hp / (float)maxHp;
        hpText.text = $"My Hp: {hp}";
        rewardText.text = $"Earn: ${reward} each shot";
        goldText.text = $"Gold: ${gold}";*/
    }
}
