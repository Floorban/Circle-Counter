using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject opponentPrefab;
    public Opponent currentOpponent;
    public GunController gun;

/*    [Header("Level Settings")]*/

    [Header("UI")]
    [SerializeField] Image hpBar;
    [SerializeField] TextMeshProUGUI hpText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        gun = FindObjectOfType<GunController>();
    }
    public void StartLevel()
    {
        gun.InitializeGun();

        GameObject opponentObject = Instantiate(opponentPrefab);
        currentOpponent = opponentObject.GetComponent<Opponent>();
        
        currentOpponent.InitializeStatus();
        UpdateUI();
    }
    public void EndLevel()
    {
        if (currentOpponent.isDead) 
        {
            Debug.Log("lose level");
        }
        else
        {
            Debug.Log("win level");
        }

        gun.animator.SetBool("isFinished", true);
        Destroy(currentOpponent.gameObject);
        currentOpponent = null;
    }

    public void UpdateUI()
    {
        hpBar.fillAmount = (float)currentOpponent.hp / (float)currentOpponent.maxHp;
        hpText.text = $"ITS Health: {currentOpponent.hp}";
    }
/*    public void EmptyChamber()
    {
        if (gun.currentBulletsList.Count > 0)
        gun.animator.SetTrigger("Empty");
    }*/

}
