using UnityEngine;
using System.Collections;
using System.Net;

public class StatusEffectHandler : MonoBehaviour
{
    public void ApplyBurn(int dps, float duration)
    {
        StartCoroutine(BurnRoutine(dps, duration));
    }

    private IEnumerator BurnRoutine(int dps, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            GetComponent<Player>()?.TakeDamage(dps);
            yield return new WaitForSeconds(1f);
            timer += 1f;
        }
    }

    public void ApplyFreeze(float duration)
    {
        StartCoroutine(FreezeRoutine(duration));
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        var controller = GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.canMove = false;
            yield return new WaitForSeconds(duration);
            controller.canMove = true;
        }
    }
}
