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
    /// <summary>
    /// The master volume for all sound effects in the game
    /// </summary>
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

    /// <summary>
    /// The master volume for all the music played in the game
    /// </summary>
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

    /// <summary>
    /// Array of sound effects used in the game
    /// </summary>
    [Tooltip("Array of sound effects used in the game")]
    public Sound[] sounds;

    /// <summary>
    /// Array of music used in the game
    /// </summary>
    [Tooltip("Array of music played in the game")]
    public Sound[] music;

    /// <summary>
    /// The singleton instance of the object.
    /// </summary>
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
    
    /// <summary>
    /// Plays a sound.
    /// </summary>
    /// <param name="name">Name of the sound</param>
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

    /// <summary>
    /// Plays a sound if it's not playing.
    /// </summary>
    /// <param name="name">The sound to be played.</param>
    /// <returns>true if the sound was playing.</returns>
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

    /// <summary>
    /// Checks if a sound is playing. 
    /// </summary>
    /// <param name="name">Name of the sound</param>
    /// <returns>true if a sound is playing.</returns>
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

    /// <summary>
    /// Stops playing a sound
    /// </summary>
    /// <param name="name">The name of the sound.</param>
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

    /// <summary>
    /// Plays a sound with random pitch
    /// </summary>
    /// <param name="name">name of the sound</param>
    /// <param name="minRange">min value of the pitch</param>
    /// <param name="maxRange">max value of the pitch</param>
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

    /// <summary>
    /// Plays a music.
    /// </summary>
    /// <param name="name">Name of the music</param>
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

    /// <summary>
    /// Stops all playing music and starts playing new music.
    /// </summary>
    /// <param name="name">Name of the music to be played.</param>
    public void PlayMusicExclusive(string name)
    {
        StopAllMusic();
        PlayMusic(name);
    }

    /// <summary>
    /// Stops all playing music.
    /// </summary>
    public void StopAllMusic()
    {
        foreach(Sound m in music)
        {
            m.source.Stop();
        }
    }

    /// <summary>
    /// Checks if the music is playing.
    /// </summary>
    /// <param name="name">Name of the music.</param>
    /// <returns>true if the music is playing.</returns>
    public bool IsMusicPlaying(string name)
    {
        return Array.Exists(music, sound => sound.name == name && sound.source.isPlaying);
    }

    /// <summary>
    /// Checks if the music from the perameter is playing.
    /// If it's playing, the method does nothing.
    /// If it's not playing, the method stops all other music and starts playing the music specified in the parameter.
    /// </summary>
    /// <param name="name">Name of the music to play</param>
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
