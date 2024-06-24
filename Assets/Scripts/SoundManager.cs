using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public bool isStart;
    public Sound[] ambientSounds, sfxSounds;
    public AudioSource ambientSrc, sfxSrc;

    float nextSound = 0;

    private void Awake()
    {
        if (instance != null)
            instance = this;
    }
    private void Start()
    {
        if (!isStart) return;
        StartCoroutine(StartGame());
    }
    IEnumerator StartGame()
    {
        PlaySound("Start", 1f);
        yield return new WaitForSeconds(7.5f);
        SceneManager.LoadScene(1);
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
