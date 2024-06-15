using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] GameObject player;
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().gameObject;
    }

    void Update()
    {
        transform.LookAt(player.transform.position);
    }
}
