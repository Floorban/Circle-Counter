using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public Sound[] ambientSounds, sfxSounds;
    public AudioSource ambientSrc, sfxSrc;

    float nextSound = 0;

    private void Awake()
    {
        if (instance != null)
            instance = this;
    }
    public void RandomizeAmbient(string name, int freq, int offset)
    {
        if (Time.time >= nextSound)
        {
            PlayRand(name);
            nextSound = Time.time + UnityEngine.Random.Range(freq - offset, freq + offset);
        }
    }
    public void PlayRand(string name)
    {
        Sound s = Array.Find(ambientSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("sound clip no found");
        }
        else
        {
            float volume = UnityEngine.Random.Range(0.1f, 0.5f);
            ambientSrc.clip = s.audioClip;
            sfxSrc.volume = volume;
            ambientSrc.Play();
            Debug.Log("audio is playing");
        }
    }
    public void PlayAmbient(string name)
    {
        Sound s = Array.Find(ambientSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("sound clip no found");
        }
        else
        {
            ambientSrc.clip = s.audioClip;
            ambientSrc.volume = 0.3f;
            ambientSrc.Play();
            Debug.Log("audio is playing");
            //ambientSrc.PlayOneShot(s.audioClip);
        }
    }
    public void PlaySound(string name, float volume)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("sound clip no found");
        }
        else
        {
            sfxSrc.clip = s.audioClip;
            sfxSrc.volume = volume;
            // sfxSrc.pitch = pitch;
            sfxSrc.Play();
        }
    }
}
