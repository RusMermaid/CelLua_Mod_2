using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenURL : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenInBrowser(string link)
    {
        Application.OpenURL(link);
    }
}
