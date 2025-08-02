using UnityEngine;

public abstract class BulletEffect : ScriptableObject
{
    public string name;
    public abstract void Apply(GameObject target);
}