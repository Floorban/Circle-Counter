using UnityEngine;
using System.Collections;

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

}
