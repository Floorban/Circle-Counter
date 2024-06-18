using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chamber : MonoBehaviour
{
    public List<Hole> holes;
    [SerializeField] Inventory inventory;

    void Awake()
    {
        foreach (Transform child in transform)
        {
            Hole hole = child.GetComponent<Hole>();
            if (hole != null)
            {
                holes.Add(hole);
            }
        }
    }
    private void OnEnable()
    {
        Actions.OnHoleSelected += DisableHoles;
        Actions.OnBulletDeselected += DisableButtons;
    }
    private void OnDisable()
    {
        Actions.OnHoleSelected -= DisableHoles;
        Actions.OnBulletDeselected -= DisableButtons;
    }
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        inventory = FindObjectOfType<Inventory>();
        InitializeChamber();
    }

    void InitializeChamber()
    {
        for (int i = 0; i < holes.Count; i++)
        {
            holes[i].isFull = false;
        }
    }
/*    public void RandomizeChamber()
    {
        int selectedSlotIndex = Random.Range(0, holes.Count);
        List<Hole> originalHoles = new List<Hole>(holes);
        holes.Clear();
        holes.Add(originalHoles[selectedSlotIndex]);

        // Add the remaining slots in clockwise order
        for (int i = 1; i < originalHoles.Count; i++)
        {
            int nextIndex = (selectedSlotIndex + i) % originalHoles.Count;
            holes.Add(originalHoles[nextIndex]);
        }
    }*/

    public void ResetChamber()
    {
        for (int i = 0; i < holes.Count; i++)
        {
            holes[i].ResetHole();
            if (holes[i].myBullet != null)
            {
                inventory.ownedBullets.Add(holes[i].myBullet);
                holes[i].myBullet.gameObject.SetActive(true);
                holes[i].myBullet = null;
                holes[i].image.color = Color.white;
            }
        }

            for (int i = 0; i < inventory.ownedBullets.Count; i++)
            {
                inventory.ownedBullets[i].ResetBullet();
            }

        FindObjectOfType<Player>().reward = 0;
    }

    public void EnableHoles()
    {
        for (int i = 0; i < holes.Count; i++)
        {
            holes[i].button.enabled = true;
        }
    }
    public void DisableHoles(Hole hole)
    {
        for (int i = 0; i < holes.Count; i++)
        {
            holes[i].button.enabled = false;
        }

        hole.myBullet = inventory.selectedBullet;
        FindObjectOfType<Player>().reward += inventory.selectedBullet.reward;
        inventory.selectedBullet.gameObject.SetActive(false);
        inventory.ownedBullets.Remove(inventory.selectedBullet);
        inventory.EnableButtons();
    }

    void DisableButtons()
    {
        for (int i = 0; i < holes.Count; i++)
        {
            holes[i].button.enabled = false;
        }
    }

    public int rotations; 
    public float duration;  

    private RectTransform rectTransform;


    public void StartReloadAnimation()
    {
        //StartCoroutine(RotateCylinder());
        FindObjectOfType<RevolverController>().StartRotateCylinder(rectTransform, rotations, duration);
    }
    public void EndShootAnimation()
    {
        StartCoroutine(RotateCylinderOneBullet());
    }

    private IEnumerator RotateCylinder()
    {
        float startRotation = rectTransform.rotation.eulerAngles.z;
        float endRotation = startRotation + 360f * rotations;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float zRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / duration) % 360f;
            rectTransform.rotation = Quaternion.Euler(0, 0, zRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is exactly the start rotation
        rectTransform.rotation = Quaternion.Euler(0, 0, startRotation);
    }
    private IEnumerator RotateCylinderOneBullet()
    {
        float startRotation = rectTransform.rotation.eulerAngles.z;
        float endRotation = startRotation + 60f; // 60 degrees clockwise
        float elapsedTime = 0f;
        float duration = 0.2f; // Adjust this for how long you want the rotation to take

        while (elapsedTime < duration)
        {
            float zRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / duration) % 360f;
            rectTransform.rotation = Quaternion.Euler(0, 0, zRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is exactly 60 degrees clockwise from the start
        rectTransform.rotation = Quaternion.Euler(0, 0, endRotation % 360f);
    }
}
