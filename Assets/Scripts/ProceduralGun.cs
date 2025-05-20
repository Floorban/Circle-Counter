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

    [Header("Barrel Settings")]
    [Tooltip("The length of the barrel in centimeters. For reference: Pistol = 10cm, Rifle = 65cm, Shotgun = 70cm")]
    [SerializeField] public float barrelLength = 15f;

    [Tooltip("The diameter of the barrel in centimeters. For reference: Pistol = 1cm, Rifle = 2.5cm, Shotgun = 4cm")]
    [SerializeField] public float barrelDiameter  = 9f;

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
