using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField]
    private int maximumSoundCount = 10;
    [SerializeField]
    private float masterVolume = 1f;
    [SerializeField]
    private bool isAudioSourceMuted = false;

    [SerializeField]
    private List<AudioSource> audioSources = new List<AudioSource>();
    [SerializeField]
    private List<Audio> audioList = new List<Audio>();

    //[SerializeField]
    //private Slider musicSlider;
    //[SerializeField]
    //private Slider soundSlider;
    //[SerializeField]
    //private Toggle soundToggle;

    private void Awake()
    {
        // AudioSource componentlerini oluþtur
        for (int i = 0; i < maximumSoundCount; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(newSource);
        }
    }

    //private void Start()
    //{
    //    if (soundSlider != null)
    //    {
    //        masterVolume = SaveLoadManager.singleton.GetSoundVolume();
    //        //musicSlider.value = masterVolume;
    //        soundSlider.value = masterVolume;
    //    }
    //    if (soundToggle != null)
    //    {
    //        isAudioSourceMuted = SaveLoadManager.singleton.GetSoundMuted();
    //        soundToggle.isOn = !isAudioSourceMuted;
    //    }
    //}

    public void PlaySound(string clipName, float volume = 1f, bool randomPitch = true, bool loop = false, float moveForward = 0)
    {
        // Aktif olan AudioSource componentini bul
        AudioSource activeSource = null;
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                activeSource = audioSources[i];
                break;
            }
        }

        if (activeSource == null)
        {
            Debug.LogWarning("There is no any other Audio Source!");
            return;
        }

        for (int i = 0; i < audioList.Count; i++)
        {
            if (clipName == audioList[i].Name)
            {
                activeSource.mute = isAudioSourceMuted;
                activeSource.pitch = Random.Range(0.9f, 1.1f);

                activeSource.clip = audioList[i].Clip;
                activeSource.volume = masterVolume * volume;
                activeSource.loop = loop;

                activeSource.time += moveForward;

                activeSource.Play();
            }
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f, bool loop = false, float moveForward = 0)
    {
        // Aktif olan AudioSource componentini bul
        AudioSource activeSource = null;
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                activeSource = audioSources[i];
                break;
            }
        }

        if (activeSource == null)
        {
            //Debug.LogWarning("Maksimum ses sayýsýna ulaþýldý");
            return;
        }

        activeSource.mute = isAudioSourceMuted;
        activeSource.pitch = Random.Range(0.9f, 1.1f);

        activeSource.clip = clip;
        activeSource.volume = masterVolume * volume;
        activeSource.loop = loop;

        activeSource.time += moveForward;

        activeSource.Play();
    }

    public void StopAllSounds()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            audioSources[i].Stop();
        }
    }

    //public void SetMasterVolume(float volume)
    //{
    //    masterVolume = volume;
    //    for (int i = 0; i < audioSources.Count; i++)
    //    {
    //        bool isAudioSourcePlaying = audioSources[i].isPlaying;
    //        audioSources[i].volume = audioSources[i].volume * volume;

    //        if (isAudioSourcePlaying)
    //            audioSources[i].Play();
    //    }

    //    //SaveLoadManager.singleton.SetMusicVolume(masterVolume);
    //    SaveLoadManager.singleton.SetSoundVolume(masterVolume);
    //}

    //public void ToggleMute(bool toogleValue)
    //{
    //    for (int i = 0; i < audioSources.Count; i++)
    //    {
    //        audioSources[i].mute = !audioSources[i].mute;
    //        isAudioSourceMuted = audioSources[i].mute;
    //    }

    //    SaveLoadManager.singleton.SetSoundMuted(audioSources[0].mute);
    //    //SaveLoadManager.singleton.SetMusicMuted(audioSources[0].mute);
    //}
}
