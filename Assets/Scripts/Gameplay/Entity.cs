using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Entity : MonoBehaviour
{
    [Header("Attributes")]
    Rigidbody rb;
    public int hp;
    public int maxHp;
    public bool isDead;
    public float moveSpeed;

    [Header("Anim")]
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
        rb = GetComponent<Rigidbody>();
        player = FindAnyObjectByType<PlayerController>().gameObject;

        startPosition = transform.position;
        StartCoroutine(IdleAnim());
        InitializeStatus();
    }

    protected virtual void Update()
    {
        transform.LookAt(player.transform.position);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed);
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

    public void InitializeStatus()
    {
        hp = maxHp;
        isDead = false;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
}
