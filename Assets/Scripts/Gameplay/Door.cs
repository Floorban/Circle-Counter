using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    GameObject player;
    Timer timer;
    public bool isStartDoor;
    public Transform tpPoint;
    [SerializeField] string prompt;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        timer = FindObjectOfType<Timer>();
    }

    public void OnInteract(string prompt)
    {
        if (!isStartDoor)
        {
            timer.canRun = false;
            player.GetComponent<Player>().isHome = true;
            Debug.Log("level ends");
        }
        else
        {
            timer.canRun = true;
            player.GetComponent<Player>().isHome = false;
            Debug.Log("level starts");
        }

        player.SetActive(false);
        player.transform.position = tpPoint.position;
        player.SetActive(true);
        timer.ShakeTimer();
    }
    public string GetPrompt()
    {
        return prompt;
    }
}
