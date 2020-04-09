/*****************************************************************************
// File Name : GameSoundEffect.cs
// Author : Connor Wolf
// Creation Date : 2/3/2020
//
// Brief Description : 
*****************************************************************************/

using UnityEngine;

[System.Serializable]
public class GameSoundEffect
{
    /// <summary>
    /// Name of the GameSound.
    /// </summary>
    public string soundName;

    /// <summary>
    /// AudioClip that corresponds to the GameSound.
    /// </summary>
    public AudioClip clip;

    /// <summary>
    /// Should this GameSound loop?
    /// </summary>
    public bool loop = false;

    /// <summary>
    /// Interruptable
    /// </summary>
    public bool interruptable = false;

    /// <summary>
    /// Volume (Range between 0 and 1)
    /// </summary>
    [Range(0, 1)]
    public float volume = 1;

    /// <summary>
    /// Pitch (Range between -3 and 3)
    /// </summary>
    [Range(-3, 4)]
    public float pitch = 1;

    /// <summary>
    /// AudioClips that can be played alternatively to the default. Randomly selected.
    /// </summary>
    public AudioClip[] alternates;

    [HideInInspector]
    public AudioSource source;

    /// <summary>
    /// Called to play the respective sound.
    /// </summary>
    public void Play()
    {
        if (alternates.Length > 0)
        {
            int rand = Random.Range(-1, alternates.Length);
            if (rand == -1) source.clip = clip;
            else source.clip = alternates[rand];
        }

        if (interruptable)
        {
            source.Play();
         }
        else
        if (!source.isPlaying)
        {
            source.Play();
        }
        else
        if (source.isPlaying)
        {
            
        }
    }

    /// <summary>
    /// Called to stop the respective sound.
    /// </summary>
    public void Stop()
    {
        source.Stop();
    }
}

