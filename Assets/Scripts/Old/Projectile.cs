using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public bool isReal;
    public bool isUsed;
    public int dmg;
    public int price;
    public int reward;
    [Range(0, 1)] public float activeChance;
}
