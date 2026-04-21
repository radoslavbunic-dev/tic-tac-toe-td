using System;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioTemplate defaultTemplate;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource uiAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioMixer audioMixer;


    public AudioTemplate SkinTemplate { get; protected set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SetMusicOnOff(PlayerPrefs.GetInt(Prefs.MusicEnabled, 1) == 1);
        SetSFXOnOff(PlayerPrefs.GetInt(Prefs.SfxEnabled, 1) == 1);
    }

    protected virtual void OnEnable()
    {
        GameEvents.SetMusicOnOff += SetMusicOnOff;
        GameEvents.SetSFXOnOff += SetSFXOnOff;
        GameEvents.PlaySFX += PlaySFX;
        GameEvents.PlayMusic += PlayMusic;
        GameEvents.StopMusic += StopMusic;
    }

    protected virtual void OnDisable()
    {
        GameEvents.SetMusicOnOff -= SetMusicOnOff;
        GameEvents.SetSFXOnOff -= SetSFXOnOff;
        GameEvents.PlaySFX -= PlaySFX;
        GameEvents.PlayMusic -= PlayMusic;
        GameEvents.StopMusic -= StopMusic;
    }

    protected virtual void SetMusicOnOff(bool value)
    {
        audioMixer.SetFloat("MusicVolume", value ? Constants.MixerUnmutedDb : Constants.MixerMutedDb);
    }

    protected virtual void SetSFXOnOff(bool value)
    {
        audioMixer.SetFloat("SFXVolume", value ? Constants.MixerUnmutedDb : Constants.MixerMutedDb);
    }

    protected virtual void PlayMusic(string id)
    {
        AudioClip clip;
        if (SkinTemplate != null && SkinTemplate.GetMusicClip(id, out clip))
        {
            PlaySound(musicAudioSource, clip);
        }
        else if (defaultTemplate.GetMusicClip(id, out clip))
        {
            PlaySound(musicAudioSource, clip);
        }
    }

    protected virtual void StopMusic()
    {
        musicAudioSource.Stop();
    }

    protected virtual void PlaySFX(string id)
    {
        AudioClip clip;
        if (SkinTemplate != null && SkinTemplate.GetSFXClip(id, out clip))
        {
            PlaySound(sfxAudioSource, clip, true);
        }
        else if (defaultTemplate.GetSFXClip(id, out clip))
        {
            PlaySound(sfxAudioSource, clip, true);
        }
    }

    void PlaySound(AudioSource source, AudioClip clip, bool oneShot = false)
    {
        if (source == null || clip == null)
        {
            return;
        }
        if (oneShot)
        {
            source.PlayOneShot(clip);
            return;
        }
        if (!oneShot && source.clip != clip)
        {
            source.clip = clip;
            source.Play();
        }
    }
}