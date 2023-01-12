using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SplashText : MonoBehaviour
{
    private Splash splash;
    private Text text;
    private SE special;

    public Splash[] splashes;
    public TextMeshProUGUI version;
    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();

        splash = splashes[Random.Range(0,splashes.Length)];
        text.text = splash.msg;
        special = splash.special;

        if(special == SE.UpsideDown)
            GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 180f);
        else if(special == SE.Version)
            text.text = version.text + "!";
    }
}