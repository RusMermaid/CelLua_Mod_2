using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject cellNameText;
    public bool esc;

    public void clicked()
    {
        cellNameText.SetActive(false);
        menu.SetActive(true);
        GridManager.playSimulation = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && esc)
        {
            clicked();
        }
    }
}
