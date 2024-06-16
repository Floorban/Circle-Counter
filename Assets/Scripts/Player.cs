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
    public float energy;
    public float maxEnergy;
    public float energySpeed;
    public bool isDead;
    public int gold;
    public bool isHome;

    [Header("UI")]
    [SerializeField] Image energyBar;
    [SerializeField] TextMeshProUGUI EnergyText;
    [SerializeField] Image hpBar;
    [SerializeField] TextMeshProUGUI goldText;

    public bool shootSelf;
    public int reward;
    private void OnEnable()
    {
        Actions.OnLevelEnd += InitializeStatus;
    }
    private void OnDisable()
    {
        Actions.OnLevelEnd -= InitializeStatus;
    }
    void EndRound()
    {
        Debug.Log("round ends");
        SceneManager.LoadScene(0);
    }
    void Start()
    {
        InitializeStatus();
    }
    private void Update()
    {
        if (!isHome)
        energy -= Time.deltaTime * energySpeed;

        UpdateUI();
    }
    public void InitializeStatus()
    {
        hp = maxHp;
        energy = maxEnergy / 2f;
        isHome = true;
        isDead = false;
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        //UpdateUI() ;

        if (hp <= 0)
        {
            isDead = true;
            EndRound();
        }
    }
    public void UpdateEnergy()
    {
        energy += reward;
        if (energy > maxEnergy) 
        {
            energy = maxEnergy;
        }
        //UpdateUI();
    }
    public void UpdateUI()
    {
        hpBar.fillAmount = (float)hp / (float)maxHp;
        energyBar.fillAmount = energy / maxEnergy;
        EnergyText.text = $"Gain: {reward} energy each shot";
        goldText.text = $"Gold: ${gold}";
    }
}
