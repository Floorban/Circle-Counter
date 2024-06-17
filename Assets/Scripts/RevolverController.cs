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
        canShoot = false;
        isShot = false;
        shootCount = 0;
        chamber = chamberPanel.GetComponentInChildren<Chamber>();
        inventory = inventoryPanel.GetComponentInChildren<Inventory>();
        player = FindObjectOfType<Player>();

        currentCoolDown = fireCoolDown;
        inChamber = true;
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
                    FindObjectOfType<PlayerCam>().ShakeCam();
                    chamber.EndShootAnimation();
                    currentCoolDown = fireCoolDown;
                }

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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        canShoot = false;
        chamber.ResetChamber();
        bulletNum = 0;
        inventoryPanel.SetActive(true);
        canControl = false;
    }
    public void CloseChamber()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canShoot = true;
        shootCount = 0;
        inventory.DisableBulletButton();
         Actions.OnBulletDeselected();
        inventoryPanel.SetActive(false);
        canControl = true;
        RandomizeChamber();
    }
}
