using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Silence : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Mute();
    }

    void OnEnable()
    {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
		Mute();
	}

    public void Mute()
    {
		foreach (AudioListener l in FindObjectsOfType<AudioListener>())
        {
            l.enabled = false;
        }
    }
}
