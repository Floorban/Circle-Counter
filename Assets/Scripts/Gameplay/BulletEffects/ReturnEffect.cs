using UnityEngine;

[CreateAssetMenu(menuName = "BulletEffects/ReturnEffect")]
public class ReturnEffect : BulletEffect
{
    public override void Apply(GameObject target, Bullet bullet)
    {
        var statusHandler = target.GetComponent<StatusEffectHandler>();
        if (statusHandler != null)
        {
            statusHandler.ReturnBullet(bullet);
        }
    }
}
