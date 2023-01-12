using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAfterTime : MonoBehaviour
{
    private Text text;
    private bool fade;
    public float fadeAfter;
    public float fadeSpeed;
    public bool ZX;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (text.color.a == 1)
        {
            fade = false;
            CancelInvoke();

            if (ZX == false || (ZX && (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.X))))
            {
                Invoke("StartFading", fadeAfter);
                Color tmp = text.color;
                tmp.a = 0.99f;
                text.color = tmp;
            }
        }

        if (fade == true)
        {
            Color tmp = text.color;
            tmp.a -= fadeSpeed;
            text.color = tmp;
        }
    }

    void StartFading()
    {
        fade = true;
    }
}
