using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCodeButtons : MonoBehaviour
{

    public GameObject cheatWindow;

    public void Hide()
    {
        cheatWindow.SetActive(false);
    }

    public void Show()
    {
        cheatWindow.SetActive(true);
    }
}
