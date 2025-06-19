using System.Collections.Generic;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;

[CreateAssetMenu(fileName = "Procedural Gun Sound", menuName = "Procedural Gun", order = 1)]
public class ProceduralGun : ScriptableObject
{
    public List<float> ShotParameters { get; private set; }

    [Header("Gun Settings")]
    [Tooltip("The type of gun. For reference: Pistol, Rifle, Shotgun")]
    [SerializeField] EventReference GunSound;

    [Header("Ammunition Settings")]
    [Tooltip("The type of ammunition. Bullet is regular ammunition while light creates ray gun-like sounds as it uses light")]
    [SerializeField] public AmmunitionType ammunition = AmmunitionType.Bullet;

    [Tooltip("The size of the caliber in millimeters")]
    [SerializeField] public float ammmoSize = 9f;

    [Tooltip("The weight of the caliber in kilograms. For reference: Pistol = 0.2g, ")]
    [SerializeField] public float ammoWeight = 9f;

    [Header("Barrel Settings")]
    [Tooltip("The type of barrel. Muzzle Break sharpens the sound since it gets redirected to the shooter, while suppressor filters ear-sensitive frequencies to camoflauge the gunshot sound")]
    [SerializeField] public MuzzleType muzzle = MuzzleType.Normal;

    [Tooltip("The length of the barrel in centimeters. For reference: Pistol = 10cm, Rifle = 65cm, Shotgun = 70cm")]
    [SerializeField] public float barrelLength = 15f;

    [Tooltip("The diameter of the barrel in centimeters. For reference: Pistol = 1cm, Rifle = 2.5cm, Shotgun = 4cm")]
    [SerializeField] public float barrelDiameter  = 9f;

    [HideInInspector] public enum AmmunitionType { Bullet, Light };
    [HideInInspector] public enum MuzzleType { Normal, MuzzleBreak, Suppressor };

    private EventInstance i_GunSound;
    public void PlaySound(
        Vector3 playPosition

    ) {
        if (!GunSound.IsNull) {
            i_GunSound = AudioManager.Instance.CreateInstance(GunSound, playPosition);

            i_GunSound.setParameterByName("BarrelLength", barrelLength);
            i_GunSound.setParameterByName("BarrelDiameter", barrelDiameter);

            i_GunSound.start();
            i_GunSound.release();
        }
        else Debug.Log("Sound not found: " + GunSound);

    }

}
