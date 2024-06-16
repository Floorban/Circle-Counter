using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Timer : MonoBehaviour
{
    [Header("Timer Behavior")]
    public bool canRun;
    public float currentTime;
    [SerializeField] Image clockBar;
    [SerializeField] Image clockBg;
    public float countDown;
    float rotationSpeed;
    //public GameObject failState;

    [Header("Animation")]
    bool canShake;
    [SerializeField] AnimationCurve shakeCurve;
    [SerializeField] float duration = 1f;
    [SerializeField] float intensity = 1f;
    bool[] quarterReminded = new bool[4];

    private void Start()
    {
        StartCoroutine(StartLevel());
    }
    void Update()
    {
        if (!canRun) return;
        if (countDown <= 0) return;

        UpdateTime();
        UpdateImage();
    }
    IEnumerator StartLevel()
    {
        currentTime = 0;
        canRun = false;
        rotationSpeed = 360f / countDown;
        //failState.SetActive(false);

        yield return null;
    }
    void UpdateTime()
    {
        currentTime += Time.deltaTime;
        Reminder(currentTime, countDown);
    }
    void UpdateImage()
    {
        float rotationAngle = currentTime * rotationSpeed;
        clockBar.transform.rotation = Quaternion.Euler(0f, 0f, -rotationAngle);
        clockBg.fillAmount = currentTime / (360f / rotationSpeed);
    }
    void Reminder(float time, float maxTime)
    {
        float quarterTime = maxTime / 4;

        for (int i = 0; i < 4; i++)
        {
            if (!quarterReminded[i] && time >= (i + 1) * quarterTime)
            {
                StartCoroutine(Shake());
                quarterReminded[i] = true; 
            }
        }

        if (time >= maxTime)
        {
            StartCoroutine(Shake());
            canRun = false;
            Actions.OnLevelEnd.Invoke();

            //failState.SetActive(true);
            //Invoke(nameof(ReturnToWorldMap), 100);
        }
    }
    public void ShakeTimer()
    {
        StartCoroutine(Shake());
    }
    IEnumerator Shake()
    {
        Vector2 startPos = new Vector2(transform.position.x, transform.position.y);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float _intensity = shakeCurve.Evaluate(elapsedTime / duration) * intensity;
            transform.position = startPos + Random.insideUnitCircle * _intensity;
            yield return null;
        }

        transform.position = startPos;
    }
}
