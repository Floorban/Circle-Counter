using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RevolverController : MonoBehaviour, IDrinkEffect
{
    [Header("Chamber")]
    Chamber chamber;
    Inventory inventory;
    Player player;
    bool inChamber;
    [SerializeField] GameObject chamberPanel;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject shopPanel;
    public List<Hole> currentHoles;
    public int bulletNum;
    [SerializeField] TextMeshProUGUI bulletNumText;

    [Header("Shooting Logic")]
    public bool canControl, canShoot, isShot, canChamber;
    int shootCount = 0;
    public float fireCoolDown;
    float currentCoolDown;
    [SerializeField] float shootRange;
    [SerializeField] Transform playerCam;
    [SerializeField] Animator gunAnimator;
    [SerializeField] Animator self_gunAnimator;
    public ParticleSystem shootPrc1;
    public ParticleSystem shootPrc2;
    public Transform shootTran1;
    public Transform shootTran2;
    public GameObject bulletPrefab;

    [Header("Pause Menu")]
    public bool hasStarted;
    [SerializeField] GameObject pausePanel;
    public bool isPaused, canPause, canInteract;

    /*    private void OnEnable()
        {
            Actions.OnPanelOpen += ToggleCanPause;
            Actions.OnPanelClosed += ToggleCanPause;
        }
        private void OnDisable()
        {
            Actions.OnPanelOpen -= ToggleCanPause;
            Actions.OnPanelClosed -= ToggleCanPause;
        }*/
    void Start()
    {
        canPause = true;
        canInteract = true;
        pausePanel.SetActive(false);
        StartCoroutine(SetUp());
        shootCount = 0;
        chamber = chamberPanel.GetComponentInChildren<Chamber>();
        inventory = inventoryPanel.GetComponentInChildren<Inventory>();
        player = FindObjectOfType<Player>();

        currentCoolDown = fireCoolDown;
        /*        canShoot = false;
                isShot = false;
                inChamber = true;*/
        canControl = true;
        canShoot = true;
        inChamber = false;

        OnGameStart();
    }
    private void Update()
    {
        if (!hasStarted) return;
   
        if (Input.GetKeyDown(KeyCode.R))
        {
            CheckChamber();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        bulletNumText.text = $"{bulletNum} / 6";
        currentCoolDown -= Time.deltaTime;

        HandleControl();
    }
    public void ApplyEffect(Drink drink)
    {
        switch (drink)
        {
            case MaxHpDrink healthDrink:
                // Handle healthDrink effect on PlayerController if needed
                break;
            case EnergyDrink energyDrink:
                // Handle energyDrink effect on PlayerController if needed
                break;
            case DamageDrink damageDrink:
                break;
        }
    }
    public void StartRotateCylinder(RectTransform trans, int rot, float dur)
    {
        StartCoroutine(RotateCylinder(trans, rot, dur));
    }
    IEnumerator RotateCylinder(RectTransform rectTransform, int rotations, float duration)
    {
        float startRotation = rectTransform.rotation.eulerAngles.z;

        int currentHoleIndex = chamber.holes.IndexOf(currentHoles[0]);

        float anglePerHole = 360f / chamber.holes.Count;
        float targetRotation = currentHoleIndex * anglePerHole;

        float endRotation = startRotation + 360f * rotations + targetRotation;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float zRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / duration) % 360f;
            rectTransform.rotation = Quaternion.Euler(0, 0, zRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.rotation = Quaternion.Euler(0, 0, targetRotation);
        Debug.Log(targetRotation);
    }
    void HandleControl()
    {
        if (!canControl) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (canShoot)
            {
                if (currentCoolDown <= 0f)
                {
                    TryShoot();
                    /*                    gunAnimator.SetTrigger("Shoot");
                                        self_gunAnimator.SetTrigger("Shoot");
                                        shootPrc1.Play();
                                        shootPrc2.Play();
                                        chamber.EndShootAnimation();*/
                    currentCoolDown = fireCoolDown;
                }

                /// manual shoot setting
                /*  if (!canShoot && !isShot)
                  {
                      gunAnimator.SetTrigger("Ready");
                      self_gunAnimator.SetTrigger("Ready");

                      canShoot = true;
                  }
                  else if (canShoot && !isShot)
                  {
                      if (currentCoolDown <= 0f)
                      {
                          TryShoot();
                          gunAnimator.SetTrigger("Shoot");
                          self_gunAnimator.SetTrigger("Shoot");

                          currentCoolDown = fireCoolDown;
                          isShot = true;
                      }
                  }
                  else if (isShot)
                  {
                      gunAnimator.SetTrigger("Reload");
                      self_gunAnimator.SetTrigger("Reload");

                      canShoot = false;
                      isShot = false;
                  }*/
            }
        }
    }
    public void RandomizeChamber()
    {
        int selectedSlotIndex = Random.Range(0, chamber.holes.Count);
        List<Hole> originalHoles = new List<Hole>(chamber.holes);
        currentHoles.Clear();
        currentHoles.Add(originalHoles[selectedSlotIndex]);

        // Add the remaining slots in clockwise order
        for (int i = 1; i < originalHoles.Count; i++)
        {
            int nextIndex = (selectedSlotIndex + i) % originalHoles.Count;
            currentHoles.Add(originalHoles[nextIndex]); ;
        }
        for (int i = 0; i < currentHoles.Count; i++)
        {
            currentHoles[i].image.color = Color.gray;
        }
        chamber.StartReloadAnimation();
    }
    public void TryShoot()
    {
        if (currentHoles.Count <= 0) return;
        /*
                gunAnimator.SetTrigger("Shoot");
                self_gunAnimator.SetTrigger("Shoot");
                shootPrc1.Play();
                shootPrc2.Play();
                chamber.EndShootAnimation();*/
        Bullet currentBullet = currentHoles[0].myBullet;

        if (currentBullet != null)
        {

            if (currentBullet.isReal)
            {
                if (player.shootSelf)
                {
                    player.TakeDamage(currentBullet.dmg);
                    Destroy(currentBullet.gameObject);
                    inventory.ownedBullets.Remove(currentBullet);
                }
                else
                {
                    ShootAt(currentBullet);
                    Destroy(currentBullet.gameObject);
                    inventory.ownedBullets.Remove(currentBullet);
                }
            }
            else
            {
                Debug.Log("fake bullet");
            }

            gunAnimator.SetTrigger("Shoot");
            self_gunAnimator.SetTrigger("Shoot");
            shootPrc1.Play();
            shootPrc2.Play();
            FindObjectOfType<SoundManager>().PlaySound("Fire", 1);
            FindObjectOfType<PlayerCam>().ShakeCam();
            isShot = true;
            StartCoroutine(InstantiateBulletVisual());
        }
        else
        {
            Debug.Log("no bullet");
            isShot = false;
            gunAnimator.SetTrigger("DryFire");
            self_gunAnimator.SetTrigger("DryFire");
            FindObjectOfType<SoundManager>().PlaySound("DryFire", 1);
        }

        shootCount++;
        currentHoles[0].image.color = Color.black;
        currentHoles.RemoveAt(0);
        chamber.EndShootAnimation();
        if (bulletNum > 0)
        {
            if (player.shootSelf) player.UpdateEnergy();

            if (isShot)
            {
                bulletNum--;
                isShot = false;
            }
        }
    }
    IEnumerator InstantiateBulletVisual()
    {
        yield return new WaitForSeconds(0.5f);

        if (player.shootSelf)
        {
            Instantiate(bulletPrefab, shootTran2.position, Quaternion.identity);
        }
        else
        {
            Instantiate(bulletPrefab, shootTran1.position, Quaternion.identity);
        }

        yield return null;
    }
    void ShootAt(Bullet bullet)
    {
        Ray gunRay = new Ray(playerCam.position, playerCam.forward);
        if (Physics.Raycast(gunRay, out RaycastHit hitInfo, shootRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out EnemyAgent enemy))
            {
                enemy.TakeDamage(bullet.dmg);
            }
        }
    }
    public void CheckChamber()
    {
        if (!canChamber) return;

        if (inChamber)
        {
            CloseChamber();
            inChamber = false;
        }
        else
        {
            OpenChamber();
            inChamber = true;
        }
    }
    public void OpenChamber()
    {
        if (inChamber) return;

        FindAnyObjectByType<PlayerCam>().LockCam();
        canShoot = false;
        chamber.ResetChamber();
        bulletNum = 0;
        inventoryPanel.SetActive(true);
        canControl = false;
        FindObjectOfType<SoundManager>().PlaySound("OpenChamber", 0.5f);
        player.GetComponent<PlayerController>().canMove = false;
    }
    public void CloseChamber()
    {
        if (!inChamber) return;

        FindAnyObjectByType<PlayerCam>().UnlockCam();
        canShoot = true;
        shootCount = 0;
        inventory.DisableBulletButton();
        Actions.OnBulletDeselected();
        inventoryPanel.SetActive(false);
        canControl = true;
        RandomizeChamber();
        FindObjectOfType<SoundManager>().PlaySound("RollChamber", 1);
        gunAnimator.SetTrigger("Ready");
        self_gunAnimator.SetTrigger("Ready");
        player.GetComponent<PlayerController>().canMove = true;
    }
    public void OpenChamberP()
    {
        canChamber = false;
        FindAnyObjectByType<PlayerCam>().LockCam();
        canShoot = false;
        chamber.ResetChamber();
        bulletNum = 0;
        inventoryPanel.SetActive(true);
        canControl = false;
        player.GetComponent<PlayerController>().canMove = false;
        chamberPanel.SetActive(false);
    }
    public void CloseChamberP()
    {
        canChamber = true;
        FindAnyObjectByType<PlayerCam>().UnlockCam();
        canShoot = true;
        shootCount = 0;
        inventory.DisableBulletButton();
        Actions.OnBulletDeselected();
        inventoryPanel.SetActive(false);
        canControl = true;
        player.GetComponent<PlayerController>().canMove = true;
        chamberPanel.SetActive(true);
    }
    public void OnGameStart()
    {
        canShoot = false;
        canControl = false;
        canChamber = false;
        player.GetComponent<PlayerController>().canMove = false;
        FindAnyObjectByType<PlayerCam>().LockCam();
        isPaused = false;
        canPause = false;
        canInteract = true;
    }
    public void PauseGame()
    {
        if (!isPaused && canPause)
        {
            canShoot = false;
            canControl = false;
            canChamber = false;
            FindAnyObjectByType<PlayerCam>().LockCam();
            player.GetComponent<PlayerController>().canMove = false;
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            canPause = false;
            canInteract = false;
        }
        else if (isPaused && !canPause)
        {
            canControl = true;
            canChamber = true;
            FindAnyObjectByType<PlayerCam>().UnlockCam();
            player.GetComponent<PlayerController>().canMove = true;
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            canPause = true;
            canInteract = true;
        }
    }
    public void OnAmmoShopPause(bool _isPaused, GameObject _shopPanel)
    {
        if (!canInteract) return;

        if (!_isPaused)
        {
            canShoot = false;
            canControl = false;
            canChamber = false;
            FindAnyObjectByType<PlayerCam>().LockCam();
            player.GetComponent<PlayerController>().canMove = false;
            _shopPanel.SetActive(true);
            OpenChamberP();
            Time.timeScale = 0f;
            canPause = false;
            FindObjectOfType<SoundManager>().PlayAmbient("TraderVoice1");
        }
        else if (_isPaused)
        {
            canControl = true;
            canChamber = true;
            FindAnyObjectByType<PlayerCam>().UnlockCam();
            player.GetComponent<PlayerController>().canMove = true;
            _shopPanel.SetActive(false);
            CloseChamberP();
            Time.timeScale = 1f;
            canPause = true;
        }
    }
    public void OnShopPause(bool _isPaused, GameObject _shopPanel)
    {
        if (!canInteract) return;

        if (!_isPaused)
        {
            canShoot = false;
            canControl = false;
            canChamber = false;
            FindAnyObjectByType<PlayerCam>().LockCam();
            player.GetComponent<PlayerController>().canMove = false;
            _shopPanel.SetActive(true);
            Time.timeScale = 0f;
            canPause = false;
            FindObjectOfType<SoundManager>().PlayAmbient("TraderVoice1");
        }
        else if (_isPaused)
        {
            canControl = true;
            canChamber = true;
            FindAnyObjectByType<PlayerCam>().UnlockCam();
            player.GetComponent<PlayerController>().canMove = true;
            _shopPanel.SetActive(false);
            Time.timeScale = 1f;
            canPause = true;
        }
    }
    IEnumerator SetUp()
    {
        yield return new WaitForSeconds(0.1f);

        inventoryPanel.SetActive(false);
        shopPanel.SetActive(false);
    }
}
