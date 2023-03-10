using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public bool esc;
    public string scene;

    public void PressButton(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeTrack()
    {
        FindObjectOfType<MusicManager>().PickRandom();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && esc)
        {
            PressButton(scene);
        }
    }
}
