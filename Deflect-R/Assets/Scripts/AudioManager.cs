/*****************************************************************************
// File Name : AudioManager.cs
// Author : 100% Connor Wolf
// Creation Date : 2/3/2020
//
// Brief Description : Manages the audio of the project and allows other
//                      objects to play audio without AudioSources. Reliant on GameSoundEffect.cs
//
// Usage: AudioManager.instance.Play("SoundNameHere");
*****************************************************************************/
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    /// <summary>
    /// Array of GameSounds that this AudioManager manages.
    /// </summary>
    public GameSoundEffect[] gameSounds;

    ///<summary>
    ///Dummy variable for initial AudioSource.
    ///</summary>
    private AudioSource settingSource;

    /// <summary>
    /// Prevents the AudioManager from being destroyed.
    /// </summary>
    protected void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        instance = this;

        foreach (GameSoundEffect gs in gameSounds)
        {
            settingSource = gameObject.AddComponent<AudioSource>();
            settingSource.clip = gs.clip;
            settingSource.volume = gs.volume;
            settingSource.pitch = gs.pitch;
            settingSource.loop = gs.loop;
            settingSource.playOnAwake = false;

            gs.source = settingSource;
        }
    }

    /// <summary>
    /// Instantiates Audio Source components for each GameSound 
    /// </summary>
    protected void Start()
    {

    }

    /// <summary>
    /// Plays the Game Sound with the matching soundName
    /// </summary>
    /// <param name="soundName"></param>
    public void PlaySound(string soundName)
    {
        foreach (GameSoundEffect gs in gameSounds)
        {
            if (soundName == gs.soundName)
            {
                gs.Play();
                return;
            }
        }

        //Called if sound doesn't exist.
        Debug.LogWarning("No GameSoundEffect called \"" + soundName + "\" exists in the current AudioManager.");
    }

    /// <summary>
    /// Stops the Game Sound with the matching soundName
    /// </summary>
    /// <param name="soundName"></param>
    public void StopSound(string soundName)
    {
        foreach (GameSoundEffect gs in gameSounds)
        {
            if (soundName == gs.soundName)
            {
                gs.Stop();
                return;
            }
        }

        //Called if sound doesn't exist.
        Debug.LogWarning("No GameSoundEffect called \"" + soundName + "\" exists in the current AudioManager.");
    }
}
