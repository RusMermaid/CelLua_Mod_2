using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using AssemblyCSharp.Assets.Assets.Scripts.Enums;

public class SetBGColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string folderName = PlayerPrefs.GetString("Texture", "Default");

        if (File.Exists(string.Concat(new string[]
        {
            Application.dataPath,
            "/texturepacks/",
            folderName,
            "/pack.json"
        })))
        {
            string path = string.Concat(new string[]
            {
                Application.dataPath,
                "/texturepacks/",
                folderName,
                "/pack.json",
            });
            string jsonString = File.ReadAllText (path); 
            TexturePackData t = JsonUtility.FromJson<TexturePackData> (jsonString);

            GetComponent<Camera>().backgroundColor = new Color(t.bgColor[0] / 255f, t.bgColor[1] / 255f, t.bgColor[2] / 255f, 1f);
        }
        
    }
}
