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
    public bool canShoot;
    int shootCount = 0;
    public float fireCoolDown;
    float currentCoolDown;
    [SerializeField] float shootRange;
    [SerializeField] Transform playerCam;
    void Start()
    {
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

        if (Input.GetMouseButtonDown(0)) 
        {
            if (canShoot && shootCount < 6)
            {
                if (currentCoolDown <= 0f)
                {
                    TryShoot();
                    currentCoolDown = fireCoolDown;
                }
            }
            else
            {
                Debug.Log("can't shoot no more");
            }
        }

        currentCoolDown -= Time.deltaTime;
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
            currentHoles[i].image.color = Color.black;
        }

        int halfSize = currentHoles.Count / 2;
        currentHoles.RemoveRange(0, halfSize);
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
            shootCount++;
        }
        else
        {
            Debug.Log("no bullet");
        }

        currentHoles.RemoveAt(0);

        if (bulletNum > 0)
        {
            player.UpdateEnergy();
        }
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
    }
    public void CloseChamber()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canShoot = true;
        shootCount = 0;
        inventoryPanel.SetActive(false);
        RandomizeChamber();
    }
}