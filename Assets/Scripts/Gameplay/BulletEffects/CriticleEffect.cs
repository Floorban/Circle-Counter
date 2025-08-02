using UnityEngine;

[CreateAssetMenu(menuName = "BulletEffects/CriticleEffect")]
public class CriticleEffect : BulletEffect
{
    public float chance;
    public override void Apply(GameObject target, Bullet bullet)
    {
        var statusHandler = target.GetComponent<StatusEffectHandler>();
        if (statusHandler != null)
        {
            statusHandler.ApplyCritical(chance, bullet);
        }
    }
}
