using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SongEE : MonoBehaviour
{
    void Update()
    {
		foreach (Image i in FindObjectsOfType<Image>())
        {
            i.color = new Color(0, 0, 1, i.color.a);
        }
		foreach (TextMeshProUGUI i in FindObjectsOfType<TextMeshProUGUI>())
        {
            i.color = new Color(0, 0, 1, i.color.a);
        }
		foreach (TextMeshPro i in FindObjectsOfType<TextMeshPro>())
        {
            i.color = new Color(0, 0, 1, i.color.a);
        }
		foreach (Text i in FindObjectsOfType<Text>())
        {
            i.color = new Color(0, 0, 1, i.color.a);
        }
    }
}
