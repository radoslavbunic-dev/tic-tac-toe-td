using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioTemplate", menuName = "Templates/Audio")]
[Serializable]
public class AudioTemplate : Template
{
    [SerializeField] protected SoundKeyPair[] music;
    [SerializeField] protected SoundKeyPair[] sfx;

    protected Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    public bool GetMusicClip(string id, out AudioClip clip)
    {
        if (clips.TryGetValue(id, out clip))
        {
            return true;
        }
        
        for (int i = 0; i < music.Length; i++)
        {
            var kvp = music[i];
            if (kvp.Key == id)
            {
                clip = kvp.Value;
                clips[kvp.Key] = clip;
                return true;
            }
        }
        return false;
    }

    public bool GetSFXClip(string id, out AudioClip clip)
    {
        if (clips.TryGetValue(id, out clip))
        {
            return true;
        }

        for (int i = 0; i < sfx.Length; i++)
        {
            var kvp = sfx[i];
            if (kvp.Key == id)
            {
                clip = kvp.Value;
                clips[kvp.Key] = clip;
                return true;
            }
        }
        return false;
    }
}