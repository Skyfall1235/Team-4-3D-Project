using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    SO_SoundList levelSounds;
    public static SoundManager Instance { get { return instance; } }
    [SerializeField, Range(0f, 1f)]
    public float soundVolume = 1f;
    [SerializeField, Range(0f, 1f)]
    public float musicVolume = 1f;
    static SoundManager instance;
    public Dictionary<GameObject, Sound> playingSounds = new Dictionary<GameObject, Sound>();
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    public void UpdatePlayingSoundVolume()
    {

        foreach (KeyValuePair<GameObject, Sound> sound in playingSounds)
        {
            if (sound.Value.isMusic)
            {
                 sound.Key.GetComponent<AudioSource>().volume = Remap(sound.Value.volume, 0f, 1f, 0f, musicVolume);
            }
            else
            {
                sound.Key.GetComponent<AudioSource>().volume = Remap(sound.Value.volume, 0f, 1f, 0f, soundVolume);
            }
        }
        
    }
    public GameObject PlaySoundAtLocation(Vector3 location, string soundName, bool loop)
    {
        if (levelSounds != null)
        {
            Sound soundToPlay = null;
            foreach (Sound sound in levelSounds.soundList)
            {
                if (sound.soundName == soundName)
                {
                    soundToPlay = sound;
                }
            }

            if (soundToPlay != null)
            {
                GameObject newSoundGO = new GameObject(soundToPlay.soundName);
                newSoundGO.transform.position = location;
                AudioSource source = newSoundGO.AddComponent<AudioSource>();
                newSoundGO.AddComponent<SoundUpdater>();
                playingSounds.Add(newSoundGO, soundToPlay);
                source.volume = soundToPlay.isMusic ? Remap(soundToPlay.volume, 0f, 1f, 0f, musicVolume) : Remap(soundToPlay.volume, 0f, 1f, 0f, soundVolume);
                source.pitch = soundToPlay.pitch;
                source.loop = loop;
                source.clip = soundToPlay.sound;
                source.Play();

                if (!loop)
                {
                    Destroy(newSoundGO, soundToPlay.sound.length);
                }
                return newSoundGO;
            }
            else
            {
                Debug.LogWarning("No sound found with given name");
                return null;
            }
        }
        else
        {
            Debug.LogWarning("No LevelSound Scriptable Object Attached to SoundManager");
            return null;
        }
    }
    public GameObject PlaySoundOnObject(GameObject obj, string soundName, bool loop)
    {
        if (levelSounds != null)
        {
            Sound soundToPlay = null;
            foreach (Sound sound in levelSounds.soundList)
            {
                if (sound.soundName == soundName)
                {
                    soundToPlay = sound;
                }
            }

            if (soundToPlay != null)
            {
                GameObject newSoundGO = new GameObject(soundToPlay.soundName);
                AudioSource source = newSoundGO.AddComponent<AudioSource>();
                newSoundGO.AddComponent<SoundUpdater>();
                newSoundGO.GetComponent<SoundUpdater>().objectForSound =  obj;
                playingSounds.Add(newSoundGO, soundToPlay);
                source.volume = soundToPlay.isMusic ? Remap(soundToPlay.volume, 0f, 1f, 0f, musicVolume) : Remap(soundToPlay.volume, 0f, 1f, 0f, soundVolume);
                source.pitch = soundToPlay.pitch;
                source.loop = loop;
                source.clip = soundToPlay.sound;
                source.Play();
                if (!loop)
                {
                    Destroy(newSoundGO, soundToPlay.sound.length + 0.1f);
                }
                return newSoundGO;
            }
            else
            {
                Debug.LogWarning("No sound found with given name");
                return null;
            }
        }
        else
        {
            Debug.LogWarning("No LevelSound Scriptable Object Attached to SoundManager");
            return null;
        }
    }
    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
