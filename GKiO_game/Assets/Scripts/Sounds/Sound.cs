using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// A class for storing details about particular sounds.
/// </summary>
[System.Serializable]
public class Sound
{
    /// <summary>
    /// Name of the sound.
    /// </summary>
    public string name;
    /// <summary>
    /// The sound clip to be played.
    /// </summary>
    public AudioClip clip;
    /// <summary>
    /// Volume of the sound.
    /// </summary>
    [Range(0f, 1f)] public float volume;
    /// <summary>
    /// Pitch of the sound.
    /// </summary>
    [Range(0.1f, 3f)] public float pitch;
    /// <summary>
    /// True if the sound is to be looped.
    /// </summary>
    public bool loop;
    /// <summary>
    /// AudioSource which will play the clip.
    /// </summary>
    [HideInInspector] public AudioSource source;
}
