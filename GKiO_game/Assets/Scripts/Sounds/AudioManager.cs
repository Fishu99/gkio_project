using System;
using UnityEngine;

/// <summary>
/// The class for playing sound in the game.
/// It is a singleton object which is persisted between scenes via DontDestroyOnLoad.
/// The sound is divided into two cattegories: sounds and music.
/// Sounds or sound effects are short clips such as footsteps, sword crash, etc.
/// Music is played in the background.
/// </summary>
public class AudioManager : MonoBehaviour
{
    [Tooltip("The master volume for all sound effects in the game")]
    [SerializeField] private float soundVolume = 1f;
    public float SoundVolume
    {
        get => soundVolume;
        set
        {
            soundVolume = value;
            UpdateSoundVolume();
        }
    }

    [Tooltip("The master volume for all the music played in the game")]
    [SerializeField] private float musicVolume = 1f;
    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            UpdateMusicVolume();
        }
    }

    [Tooltip("Array of sound effects used in the game")]
    public Sound[] sounds;
    [Tooltip("Array of music played in the game")]
    public Sound[] music;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        ConfigureSoundsInArray(sounds);
        ConfigureSoundsInArray(music);
        UpdateMusicVolume();
        UpdateSoundVolume();
    }
    
    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public bool PlayIfNotPlayed (string name)
    {
        bool itIsPlaying = false;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return itIsPlaying;
        }
        if (!s.source.isPlaying)
        {
            s.source.Play();
            itIsPlaying = false;
        }
        else
            itIsPlaying = true;

        return itIsPlaying;
    }

    public bool CheckIfIsPlaying (string name)
    {
        bool isPlaying = false;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return isPlaying;
        }
        if (s.source.isPlaying)
            isPlaying = true;

        return isPlaying;
    }

    public void Stop (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void PlayWithRandomPitch(string name, int minRange, int maxRange)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        //Checking values range
        if (minRange > maxRange)
            maxRange = minRange;
        if (minRange < 0)
            minRange = 0;
        if (maxRange > 1)
            maxRange = 1;

        //Generate random pitch value
        int randomPercentage = UnityEngine.Random.Range(minRange, maxRange);
        float pitchValueGenerated = randomPercentage * (3.0f - 0.1f) / 100; //Pitch has values from 0.1f to 3.0f

        s.pitch = pitchValueGenerated;
        s.source.Play();
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(music, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void PlayMusicExclusive(string name)
    {
        StopAllMusic();
        PlayMusic(name);
    }

    public void StopAllMusic()
    {
        foreach(Sound m in music)
        {
            m.source.Stop();
        }
    }

    public bool IsMusicPlaying(string name)
    {
        return Array.Exists(music, sound => sound.name == name && sound.source.isPlaying);
    }

    public void PlayMusicExclusiveIfNotPlayed(string name)
    {
        if (!IsMusicPlaying(name))
        {
            PlayMusicExclusive(name);
        }
    }

    private void ConfigureSoundsInArray(Sound[] sounds)
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void UpdateSoundVolume()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume*soundVolume;
        }
    }

    private void UpdateMusicVolume()
    {
        foreach (Sound s in music)
        {
            s.source.volume = s.volume * musicVolume;
        }
    }
}
