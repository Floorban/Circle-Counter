using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCController : MonoBehaviour
{
    [Header ("Anim")]
    GameObject player;
    float idleAnimSpeed;
    float moveAmplitude;
    [SerializeField] float speedMin;
    [SerializeField] float speedMax;
    [SerializeField] float amplitudeMin;
    [SerializeField] float amplitudeMax;
    private Vector3 startPosition;
    private float timeCounter = 0;

    protected virtual void Start()
    {
        player = FindAnyObjectByType<PlayerCam>().gameObject;

        startPosition = transform.position;
        StartCoroutine(IdleAnim());
    }

    protected virtual void Update()
    {
        transform.LookAt(player.transform.position);
    }

    IEnumerator IdleAnim()
    {
        while (true)
        {
            idleAnimSpeed = Random.Range(speedMin, speedMax);
            moveAmplitude = Random.Range(amplitudeMin, amplitudeMax);

            float initialTime = timeCounter;

            // Complete one full cycle of sine wave (2 * Mathf.PI radians)
            while (timeCounter < initialTime + 2 * Mathf.PI)
            {
                timeCounter += Time.deltaTime * idleAnimSpeed;
                float newY = startPosition.y + Mathf.Sin(timeCounter) * moveAmplitude;
                transform.position = new Vector3(startPosition.x, newY, startPosition.z);

                yield return null;
            }

            timeCounter -= 2 * Mathf.PI;
        }
    }

/*    public virtual void OnInteract() 
    { 
        Debug.Log("NPCController interacted with");
    }
    public string GetPrompt()
    {
        return prompt;
    }*/
}
