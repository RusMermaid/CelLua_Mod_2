using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public GameObject menu;
    public GameObject cellNameText;
    public bool esc;

    public void clicked() {
        menu.SetActive(false);
        cellNameText.SetActive(true);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && esc)
        {
            clicked();
        }
    }
}
