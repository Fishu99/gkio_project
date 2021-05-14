using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
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

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
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
}
