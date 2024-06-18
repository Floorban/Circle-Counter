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
        //transform.LookAt(player.transform.position);
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
    public void AttackPlayer()
    {
        Vector3 kickPos = transform.position + transform.forward * 0.5f + Vector3.up * 0.87f;
        Collider[] cols = Physics.OverlapSphere(kickPos, 0.8f);
        foreach (Collider col in cols)
        {
            if (col.gameObject == gameObject) continue;
            Rigidbody cr = col.GetComponent<Rigidbody>();
            if (!cr) continue;
            Vector3 cp = col.ClosestPoint(kickPos);
            //Instantiate(kickEffect, cp, Quaternion.identity);
            if (cr.CompareTag("Player")) cr.AddForceAtPosition((cp - (transform.position - transform.forward + Vector3.down * 2f)) * 100, cp);
        }
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
