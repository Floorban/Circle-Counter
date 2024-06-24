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

public class Player : MonoBehaviour, IDrinkEffect
{
    [Header("Attributes")]
    public int hp;
    public int maxHp;
    public float sanity;
    public float maxSanity;
    public float sanitySpeed;
    public float sanitySpeedMultiplier = 1f;
    public bool isDead;
    public int gold;
    public int addedGold;
    public bool isHome, canEnd;
    [SerializeField] RawImage camOutput;

    [Header("UI")]
    [SerializeField] Image sanityBar;
    [SerializeField] TextMeshProUGUI sanityText;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] Image hpBar;
    [SerializeField] Image[] hps;
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] float lerpSpeed;
    RevolverController revolver;

    [Header("Post Processing")]
    [SerializeField] Volume volume;
    Vignette vignette;
    public float vigIntensity;

    public bool shootSelf;
    public int reward, addedReward;
    public bool avoidFirstShot;

    private void OnEnable()
    {
        Actions.OnLevelEnd += InitializeStatus;
    }
    private void OnDisable()
    {
        Actions.OnLevelEnd -= InitializeStatus;
    }
    public void EndGame()
    {
        revolver.PauseGame();
        SceneManager.LoadScene(2);
    }
    void EndRound()
    {
        Debug.Log("round ends");
        if (canEnd) 
        {
            StartCoroutine(NextRound());
            canEnd = false;
        }
    }
    IEnumerator NextRound()
    {
        camOutput.color = Color.black;
        FindObjectOfType<SoundManager>().PlayRand("Death");
        yield return new WaitForSeconds(7f);
        SceneManager.LoadScene(2);
    }
    void Start()
    {
        canEnd = true;
        if (volume.profile.TryGet(out Vignette vignette))
        {
            this.vignette = vignette;
        }
        revolver = FindObjectOfType<RevolverController>();
        hp = maxHp;
        sanity = maxSanity / 2f;
        isHome = true;
        vigIntensity = 0.85f;
    }
    private void Update()
    {
        HandleSanity();
        UpdateUI();
        UpdateVignetteIntensity();
    }
    public void InitializeStatus()
    {
        gold += addedGold;
        //hp = maxHp;
        //sanity = maxSanity / 2f;
        isHome = true;
        //isDead = false;
    }
    public void ApplyEffect(Drink drink)
    {
        switch (drink)
        {
            case AddHpDrink hpDrink:
                hp = maxHp;
                break;
            case AddSanity sanityDrink:
                sanity = maxSanity;
                break;
            case MaxHpDrink maxHpDrink:
                maxHp += maxHpDrink.healthBoost;
                break;
            case MaxSanityDrink maxSanityDrink:
                maxSanity += maxSanityDrink.sanityBoost;
                break;
            case SanitySpeedDrink sanitySpeedDrink:
                sanitySpeedMultiplier = sanitySpeedDrink.sanitySpeedBoost;
                break;
            case VisionDrink visionDrink:
                vigIntensity = visionDrink.visionBoost;
                break;
            case RewardPlusDrink rewardDrink:
                addedReward += rewardDrink.rewardBoost;
                break;
            case PiggyBankDrink bankDrink:
                 addedGold += bankDrink.goldBoost;
                break;
            case AvoidFirstShotDrink AFSDrink:
                avoidFirstShot = true;
                break;
        }
    }
    public void HandleSanity()
    {
        if (!isHome)
            sanity -= Time.deltaTime * sanitySpeed * sanitySpeedMultiplier;

        if (sanity <= 0)
            EndRound();
    }
    void UpdateVignetteIntensity()
    {
        if (vignette != null)
        {
            float targetIntensity = vigIntensity - (sanity / maxSanity);
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, lerpSpeed * Time.deltaTime);
        }
    }
    public void TakeDamage(int dmg)
    {
        if (avoidFirstShot)
        {
            avoidFirstShot = false;
            Debug.Log("First shot avoided!");
            return;
        }

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
        sanity += reward + addedReward;
        if (sanity > maxSanity) 
        {
            sanity = maxSanity;
        }
        //UpdateUI();
    }
    public void UpdateUI()
    {
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, (float)hp / (float)maxHp, lerpSpeed * Time.deltaTime);
        hpText.text = $"HP: {hp} / {maxHp}";
        /* for (int i = 0; i < hps.Length; i++) 
         {
             hps[i].enabled = !DisplayHp(hp, i);
         }*/
        sanityBar.fillAmount = sanity / maxSanity;
        goldText.text = $"${gold}";

        Color energyColor = Color.Lerp(Color.red, Color.green, (sanity / maxSanity));
        sanityBar.color = energyColor;

        if (revolver.bulletNum > 0)
        sanityText.text = $"Gain: {reward + addedReward} sanity each shot";
        else
        sanityText.text = $"No Bullets! Gain: {0} sanity each shot";
    }
}
