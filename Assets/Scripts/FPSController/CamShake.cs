using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    [Header("Cam Shake")]
    [SerializeField] AnimationCurve shakeCurve;
    public bool canShake;
    [SerializeField] float duration = 1f;
    [SerializeField] float intensity = 1f;
    void Start()
    {
        canShake = true;
    }
    void Update()
    {
        ShakeCam();
    }

    public void ShakeCam()
    {
        if (canShake)
        {
            StartCoroutine(Shake());
            canShake = false;
        }
    }
    IEnumerator Shake()
    {
        Vector2 startPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float _intensity = shakeCurve.Evaluate(elapsedTime / duration) * intensity;
            transform.localPosition = startPos + Random.insideUnitCircle * _intensity;
            yield return null;
        }

        transform.localPosition = startPos;
    }
}
