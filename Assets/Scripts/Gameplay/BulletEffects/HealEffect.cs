using UnityEngine;

[CreateAssetMenu(menuName = "BulletEffects/HealEffect")]
public class HealEffect : BulletEffect
{
    public float healAmount;
    public override void Apply(GameObject target, Bullet bullet)
    {
        var statusHandler = target.GetComponent<StatusEffectHandler>();
        if (statusHandler != null)
        {
            statusHandler.ApplyHeal(healAmount, bullet);
        }
    }
}
