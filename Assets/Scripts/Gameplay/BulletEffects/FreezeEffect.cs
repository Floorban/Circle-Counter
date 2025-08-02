using UnityEngine;

[CreateAssetMenu(menuName = "BulletEffects/FreezeEffect")]
public class FreezeEffect : BulletEffect
{
    public float duration = 3f;

    public override void Apply(GameObject target)
    {
        var statusHandler = target.GetComponent<StatusEffectHandler>();
        if (statusHandler != null)
        {
            statusHandler.ApplyFreeze(duration);
        }
    }
}
