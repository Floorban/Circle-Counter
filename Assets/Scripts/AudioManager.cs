using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

using FMOD.Studio;
using System;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Volume Control")]
    [Tooltip("The value that controls the volume for the master mixer, which contains all the sound in the game. You can only change the volume before playing.")]
    [Range(0, 100)] public float MasterVolume = 50;

    [Tooltip("The value which controls ambient volume specifically. You can only change the volume before playing.")]
    [Range(0, 100)] public float AmbientVolume = 100;

    [Tooltip("The value which controls music volume specifically. You can only change the volume before playing.")]
    [Range(0, 100)] public float MusicVolume = 100;

    [Tooltip("The value which controls sound effects volume specifically. You can only change the volume before playing.")]
    [Range(0, 100)] public float SFXVolume = 100;

    [Header("FMOD Parameters")]
    [Tooltip("The intensity of the randomness of the sound, 0% is no randomness, 100% is full randomness, this can be used to add variety to sounds, such as stylization, pitch, volume etc.")]
    [SerializeField][Range(0, 100)] private int GlobalRandomnessIntensityValue = 50;

    [Tooltip("A list of custom parameters that can be used to change the intensities for sounds, add your own custom parameter(s) here that you would like to be able to change")]
    public List<string> CustomParameter = new List<string>();
    //TODO: Dictionary for CustomParameter

    [Header("Developer Attributes")]
    [SerializeField] bool developerMode = false;
    [SerializeField] EventReference SFX_TestAudio;

    public enum GameState { MainMenu, Play, Pause }
    public GameState gameState = GameState.Play;

    /// Local Attributes
    // FMOD Parameters
    private string RandomnessIntensity = "RandomnessIntensity";
    //private string IndoorIntensity = "IndoorIntensity";

    // Audio Mixer
    private Bus MasterBus;
    private Bus AmbientBus;
    private Bus MusicBus;
    private Bus SFXBus;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
    }

    /*private void Start() {
        MasterBus = RuntimeManager.GetBus("bus:/");
        AmbientBus = RuntimeManager.GetBus("bus:/Ambient");
        MusicBus = RuntimeManager.GetBus("bus:/Music");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");

        MasterBus.setVolume(MasterVolume / 100f);
        AmbientBus.setVolume(AmbientVolume / 100f);
        MusicBus.setVolume(MusicVolume / 100f);
        SFXBus.setVolume(SFXVolume / 100f);
    }*/

    private void Update() {
        if (developerMode) if (Input.GetKeyDown(KeyCode.P)) PlaySound(SFX_TestAudio, transform.position);
    }

    public void PlaySound( // Plays a oneshot SFX with position-dependent audio, where parameters cannot be changed after initiation, ideal for short-duration SFX
        EventReference sfx,
        Vector3 playPosition,
        int randomnessIntensityValue = 1
    ) {
        if (!sfx.IsNull) {
            EventInstance sfxInstance = CreateInstance(sfx, playPosition);
            sfxInstance = CreateInstance(sfx, playPosition);
            if (randomnessIntensityValue != 0) sfxInstance.setParameterByName(RandomnessIntensity, randomnessIntensityValue);
            if (randomnessIntensityValue < 0) sfxInstance.setParameterByName(RandomnessIntensity, GlobalRandomnessIntensityValue);

            sfxInstance.start();
            sfxInstance.release();
        }
        else Debug.Log("Sound not found: " + sfx);

    }

    public EventInstance PlayInstance( // Plays a looping sound with position-dependent audio, where parameters can be changed after initiation, ideal for long-duration sounds
        EventReference sound,
        Vector3 playPosition,
        GameObject gameObject,
        int randomnessIntensityValue = -1
    ) {
        if (!sound.IsNull) {
            EventInstance loopInstance = CreateInstance(sound, playPosition);
            RuntimeManager.AttachInstanceToGameObject(loopInstance, gameObject);
            if (randomnessIntensityValue != 0) loopInstance.setParameterByName(RandomnessIntensity, randomnessIntensityValue);
            if (randomnessIntensityValue < 0) loopInstance.setParameterByName(RandomnessIntensity, GlobalRandomnessIntensityValue);
            loopInstance.start();
            return loopInstance;
        }
        else Debug.Log("Sound not found: " + sound);
        return default(EventInstance);
    }

    public EventInstance CreateInstance(EventReference audio, Vector3 eventPosition) {
        EventInstance audioInstance = RuntimeManager.CreateInstance(audio);
        audioInstance.set3DAttributes(RuntimeUtils.To3DAttributes(eventPosition));
        return audioInstance;

    }

    public void ContinueInstance(params EventInstance[] audioInstances) {
        foreach (var instance in audioInstances) {
            instance.setPaused(false);
        }
    }
    public void PauseInstance(params EventInstance[] audioInstances) {
        foreach (var instance in audioInstances) {
            instance.setPaused(true);
        }
    }

    public void StopInstance(params EventInstance[] audioInstances) {
        foreach (var instance in audioInstances) {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    public void ClearInstance(params EventInstance[] audioInstances) {
        foreach (EventInstance instance in audioInstances) {
            instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instance.release();
        }
    }

    public bool IsAudioPlaying(params EventInstance[] audioInstances) {
        foreach (EventInstance instance in audioInstances) {
            instance.getPaused(out bool paused);
            if (paused) return false;
        }
        return true;
    }


    public void SetParameter(EventInstance audioInstance, string parameterName, float value) => audioInstance.setParameterByName(parameterName, value);

    void RefreshMixer() { // Refreshes the mixer to its' default values
        AmbientBus.setMute(false); AmbientBus.setPaused(false);
        MusicBus.setMute(false); MusicBus.setPaused(false);
        SFXBus.setMute(false); SFXBus.setPaused(false);

        //MuffleAudio(false);
    }

    public void AdjustAudioToState(GameState state) { // Global parameters that change all SFX according to player state
        gameState = state;

        switch (gameState) {
            case GameState.Play:
                RefreshMixer();
                break;

            case GameState.Pause:
                RefreshMixer();
                SFXBus.setPaused(true);
                AmbientBus.setPaused(true);
                //MuffleAudio(true);
                break;

            case GameState.MainMenu:
                RefreshMixer();
                break;
        }

    }

}
