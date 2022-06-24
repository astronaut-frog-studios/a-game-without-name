using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool loop;

    [Range(0f, 1f)] public float volume;
    [Range(.1f, 3f)] public float pitch;

    [HideInInspector] public AudioSource source;
}

public class AudioSystem : Singleton<AudioSystem>
{
    [SerializeField] private Sound[] sounds;

    public readonly Dictionary<string, Sound> soundSources = new Dictionary<string, Sound>();

    protected override void Awake()
    {
        base.Awake();

        foreach (var sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;

            soundSources.Add(sound.name, sound);
        }
    }

    public void PlayMusic(string clipName)
    {
        var sound = soundSources[clipName];

        sound?.source.Play();
    }

    public void PlaySfx(string clipName)
    {
        var sound = soundSources[clipName];

        sound?.source.PlayOneShot(sound.clip);
    }

    public void StopPlaying(string clipName)
    {
        var sound = soundSources[clipName];

        sound?.source.Stop();
    }
}