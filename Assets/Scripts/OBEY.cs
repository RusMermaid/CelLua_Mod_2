using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OBEY : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
		foreach (Text text in FindObjectsOfType<Text>())
        {
            text.text = "OBEY";
        }
		foreach (TextMesh text in FindObjectsOfType<TextMesh>())
        {
            text.text = "OBEY";
        }
		foreach (TextMeshProUGUI text in FindObjectsOfType<TextMeshProUGUI>())
        {
            text.text = "OBEY";
        }
    }
}
