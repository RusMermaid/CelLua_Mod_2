using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class MusicManager : MonoBehaviour
{
    static bool debounce;

    public MusicTrack[] jukebox;

    private string nowPlaying = "";

    // Start is called before the first frame update
    void Start()
    {

        if (debounce) 
        {
            Destroy(gameObject);
            return;
        }
        debounce = true;
        DontDestroyOnLoad(gameObject);



        if (!PlayerPrefs.HasKey("Music Volume"))
        {
            PlayerPrefs.SetFloat("Music Volume", 1f);
        }

        if (!PlayerPrefs.HasKey("FX Volume"))
        {
            PlayerPrefs.SetFloat("FX Volume", 1f);
        }

        foreach (MusicTrack s in jukebox)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.altSource = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            if(s.altClip != null)
                s.altSource.clip = s.altClip;
            s.source.volume = PlayerPrefs.GetFloat("Music Volume");
            s.altSource.volume = PlayerPrefs.GetFloat("Music Volume");
            s.source.priority = 0;
        }

        PickRandom();
    }

    public void ResetSources()
    {
        while (GetComponent<AudioSource>() != null)
        {
            Destroy(GetComponent<AudioSource>());
        }

        foreach (MusicTrack s in jukebox)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.altSource = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            if(s.altClip != null)
                s.altSource.clip = s.altClip;
            s.source.volume = PlayerPrefs.GetFloat("Music Volume");
            s.altSource.volume = PlayerPrefs.GetFloat("Music Volume");
            s.source.priority = 0;
        }

        PickRandom();
    }

    public void PickRandom()
    {
        int pick = 0;
        
        pick = UnityEngine.Random.Range(0, jukebox.Length);

        while (nowPlaying == jukebox[pick].name || jukebox[pick].secret == true)
        {
            pick = UnityEngine.Random.Range(0, jukebox.Length);
        }

        Play(jukebox[pick].name);
    }

    public void Play(string name)
    {
        CancelInvoke("RemoveText");
        foreach (MusicTrack m in jukebox)
        {
            m.source.Stop();
            m.altSource.Stop();
        }

        MusicTrack s = Array.Find(jukebox, track => track.name == name);

        if(s.altClip == null)
        {
            s.source.Play();
            s.source.loop = true;
        }
        else
        {
            s.altSource.Play();
            s.source.PlayScheduled(AudioSettings.dspTime + s.altSource.clip.length);
            s.source.loop = true;
        }
        
        nowPlaying = name;

        if (s.composer == "")
            GameObject.Find("NowPlaying").GetComponent<TextMeshProUGUI>().text = "Now Playing - " + name;
        else
            GameObject.Find("NowPlaying").GetComponent<TextMeshProUGUI>().text = "Now Playing - " + name + " by " + s.composer;

        Invoke("RemoveText", 2f);
    }

    public void volumeUpdate(float vol)
    {
        foreach (MusicTrack s in jukebox)
        {
            s.altSource.volume = vol;
            s.source.volume = vol;
        }
        PlayerPrefs.SetFloat("Music Volume", vol);
    }

    void RemoveText()
    {
        if(GameObject.Find("NowPlaying") != null)
            GameObject.Find("NowPlaying").GetComponent<TextMeshProUGUI>().text = "";
    }

}
