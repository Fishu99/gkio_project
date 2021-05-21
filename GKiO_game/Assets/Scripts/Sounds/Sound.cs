using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// A class for storing details about particular sounds.
/// </summary>
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume;
    [Range(0.1f, 3f)] public float pitch;
    public bool loop;
    [HideInInspector] public AudioSource source;
}
