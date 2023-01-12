using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    public static AudioManager i;
    public static bool playSounds = true;
    AudioSource source;
    static bool debounce;

    List<AudioSource> pitchedSounds = new List<AudioSource>();

    void Start()
    {

        if (debounce)
        {
            return;
        }
        debounce = true;
        DontDestroyOnLoad(gameObject);

        i = this;
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        foreach (AudioSource s in pitchedSounds.ToList())
        {
            if (!s.isPlaying)
            {
                pitchedSounds.Remove(s);
                Destroy(s);
            }
        }
    }

    public void PlaySound(AudioClip sound, float pitch = 1, bool pitched = false) {
        if (!playSounds)
            return;

        if (!pitched)
        {
            source.volume = PlayerPrefs.GetFloat("FX Volume");
            source.PlayOneShot(sound);
        }
        else
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.volume = PlayerPrefs.GetFloat("FX Volume");
            newSource.pitch = pitch;
            newSource.PlayOneShot(sound);
            pitchedSounds.Add(newSource);
        }
    }
}
