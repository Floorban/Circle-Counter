using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RevolverController : MonoBehaviour
{
    [Header ("Chamber")]
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
    public bool canControl, canShoot, isShot;
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
    void Start()
    {
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
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CheckChamber();
        }

        bulletNumText.text = $"{bulletNum} / 6";
        currentCoolDown -= Time.deltaTime;

        HandleControl();
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
                    gunAnimator.SetTrigger("Shoot");
                    self_gunAnimator.SetTrigger("Shoot");
                    shootPrc1.Play();
                    shootPrc2.Play();
                    chamber.EndShootAnimation();
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
            currentHoles.Add(originalHoles[nextIndex]);;
        }
        for (int i = 0; i < currentHoles.Count; i++)
        {
            currentHoles[i].image.color = Color.black;
        }
        chamber.StartReloadAnimation();
    }
    public void TryShoot()
    {
        if (currentHoles.Count <= 0) return;

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

            FindObjectOfType<SoundManager>().PlaySound("Fire", 1);
            FindObjectOfType<PlayerCam>().ShakeCam();
            bulletNum--;
            StartCoroutine(InstantiateBulletVisual());
        }
        else
        {
            Debug.Log("no bullet");
        }

        shootCount++;
        currentHoles.RemoveAt(0);

        if (player.shootSelf && bulletNum >= 0)
        {
            player.UpdateEnergy();
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
            if (hitInfo.collider.gameObject.TryGetComponent(out Entity enemy)) 
            {
                enemy.TakeDamage(bullet.dmg);
            }
        }
    }
    public void CheckChamber()
    {
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
        FindAnyObjectByType<PlayerCam>().LockCam();
        canShoot = false;
        chamber.ResetChamber();
        bulletNum = 0;
        inventoryPanel.SetActive(true);
        canControl = false;
    }
    public void CloseChamber()
    {
        FindAnyObjectByType<PlayerCam>().UnlockCam();
        canShoot = true;
        shootCount = 0;
        inventory.DisableBulletButton();
         Actions.OnBulletDeselected();
        inventoryPanel.SetActive(false);
        canControl = true;
        RandomizeChamber();

        gunAnimator.SetTrigger("Ready");
        self_gunAnimator.SetTrigger("Ready");
    }

    IEnumerator SetUp()
    {
        yield return new WaitForSeconds(0.1f);

        inventoryPanel.SetActive(false);
        shopPanel.SetActive(false);
    }
}
