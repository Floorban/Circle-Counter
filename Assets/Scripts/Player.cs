using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    [Header("Attributes")]
    public int hp;
    public int maxHp;
    public float energy;
    public float maxEnergy;
    public float energySpeed;
    public float energyMultiplier = 1f;
    public bool isDead;
    public int gold;
    public bool isHome;

    [Header("UI")]
    [SerializeField] Image energyBar;
    [SerializeField] TextMeshProUGUI EnergyText;
    [SerializeField] Image hpBar;
    [SerializeField] Image[] hps;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] float lerpSpeed;

    [Header("Post Processing")]
    [SerializeField] Volume volume;
    Vignette vignette;

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
        StartCoroutine(NextRound());
    }
    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(0.07f);
        SceneManager.LoadScene(0);
    }
    void Start()
    {
        if (volume.profile.TryGet(out Vignette vignette))
        {
            this.vignette = vignette;
        }
        InitializeStatus();
    }
    private void Update()
    {
        HandleSanity();
        UpdateUI();
        UpdateVignetteIntensity();
    }
    public void InitializeStatus()
    {
        hp = maxHp;
        energy = maxEnergy / 2f;
        isHome = true;
        isDead = false;
    }
    public void HandleSanity()
    {
        if (!isHome)
            energy -= Time.deltaTime * energySpeed * energyMultiplier;

        if (energy <= 0)
            EndRound();
    }
    void UpdateVignetteIntensity()
    {
        if (vignette != null)
        {
            float targetIntensity = 0.8f - (energy / maxEnergy);
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, lerpSpeed * Time.deltaTime);
        }
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        //UpdateUI() ;

        if (hp <= 0)
        {
            isDead = true;
            //FindObjectOfType<SoundManager>().PlaySound("Fire", 1f);
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
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, (float)hp / (float)maxHp, lerpSpeed * Time.deltaTime);
        /* for (int i = 0; i < hps.Length; i++) 
         {
             hps[i].enabled = !DisplayHp(hp, i);
         }*/
        energyBar.fillAmount = energy / maxEnergy;
        EnergyText.text = $"Gain: {reward} sanity each shot";
        goldText.text = $"${gold}";

        Color energyColor = Color.Lerp(Color.red, Color.green, (energy / maxEnergy));
        energyBar.color = energyColor;
    }
}
