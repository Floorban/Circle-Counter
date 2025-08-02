using UnityEngine;

[CreateAssetMenu(menuName = "BulletEffects/BurnEffect")]
public class BurnEffect : BulletEffect
{
    public int damagePerSecond = 5;
    public float duration = 3f;

    public override void Apply(GameObject target)
    {
        var statusHandler = target.GetComponent<StatusEffectHandler>();
        if (statusHandler != null)
        {
            statusHandler.ApplyBurn(damagePerSecond, duration);
        }
    }
}