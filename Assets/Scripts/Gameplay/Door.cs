using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    GameObject player;
    Timer timer;
    public bool isStartDoor;
    public Transform tpPoint;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        timer = FindObjectOfType<Timer>();
    }

    public void OnInteract()
    {
        if (timer != null)
        {
            if (!isStartDoor) 
            {
                timer.canRun = false;
                Debug.Log("level ends");
            }
            else
            {
                timer.canRun = true;
                Debug.Log("level starts");
            }

            timer.ShakeTimer();
            player.transform.position = tpPoint.position;
        }
    }

}
