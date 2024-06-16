using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;
    public Player player;

    public int reward;

    [Header("UI")]
    [SerializeField] Image hpBar;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] TextMeshProUGUI goldText;
/*    private void OnEnable()
    {
        Actions.OnDamageCaused += UpdateUI;
        Actions.OnLevelFinished += EndRound;
    }
    private void OnDisable()
    {
        Actions.OnDamageCaused -= UpdateUI;
        Actions.OnLevelFinished -= EndRound;
    }*/
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }
    public void UpdateGold()
    {
        player.gold += reward;
        UpdateUI();
    }
    public void UpdateUI()
    {
        hpBar.fillAmount = (float)player.hp / (float)player.maxHp;
        hpText.text = $"My Hp: {player.hp}";
        rewardText.text = $"Earn: ${reward} each shot";
        goldText.text = $"Gold: ${player.gold}";
    }
    void EndRound()
    {
        Debug.Log("round ends");
        SceneManager.LoadScene(0);
    }
}
