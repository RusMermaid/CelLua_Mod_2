using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class MusicTrack
{   
    [HideInInspector]
    public AudioSource source;
    [HideInInspector]
    public AudioSource altSource;

    public AudioClip clip;
    public AudioClip altClip;
    [TextArea]
    public string name;
    [TextArea]
    public string composer;

    public bool secret;
}
