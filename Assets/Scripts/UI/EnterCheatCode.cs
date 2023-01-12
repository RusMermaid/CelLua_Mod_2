using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System.Security.Cryptography;
using TMPro;

public class EnterCheatCode : MonoBehaviour
{
    private InputField input;
    public GameObject crabScreen;
    public GameObject OBEY;
    public GameObject silence;
    public GameObject playerEE;
    public GameObject songEE;

    public string[] types;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<InputField>();
        Debug.Log(SHA256Hash("HALLOWEEN"));
        Debug.Log(SHA256Hash("SKELETON"));
        Debug.Log(SHA256Hash("SPOOKYSCARYSKELETONS"));
        Debug.Log(SHA256Hash("SPOOKYMONTH"));
        Debug.Log(SHA256Hash("SPOOKTOBER"));
    }

    public string SHA256Hash(string data)
    {
        SHA256 sha = new SHA256Managed ();
        byte[] hash = sha.ComputeHash (Encoding.ASCII.GetBytes (data));
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in hash) {
            stringBuilder.AppendFormat ("{0:x2}", b);
        }
	    return stringBuilder.ToString ();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string i = SHA256Hash(input.text.ToUpper());

            if (i == "da9c2566db5b5753bbef05463544fd5b211f0d8eb18724109a0643c6fc3a57d3" || i == "3031ff21446d68533d06c21adfa60b6d449ad4b938180a582ab97f962a75a8e2"
            || i == "b87219f122e62e4444810a211c5dad92741544774cdf0f5fe1be01c19ca25380" || i == "864c062829ce22f79754ee3cfeeef19617c72152a20473a39e1f739095f42dec")
            {
                crabScreen.SetActive(true);
                FindObjectOfType<CheatCodeButtons>().Hide();
                input.text = "";
            }
            else if (i == "364261e4daabde3688ef49e8d767d74d1b4b5c30fb8decfff8b8d8c92809e648" || i == "33faaf678c41b3ec12338f919afe1884ef075d51869f98a039fb87ccec61c7b4")
            {
                Application.OpenURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
                FindObjectOfType<CheatCodeButtons>().Hide();
                input.text = "";
            }
            else if (i == "c1713211e17d63664a692c41eafd86a0901370e0102632dd2818584e95d0394b" || i == "5408a18b69913c810d081047044684124abbae92a12e0a0e49e74b2760e1b0b5")
            {
                Application.OpenURL("https://www.roblox.com/games/8821714674/anomalocaris-worshiping-simulator");
                FindObjectOfType<CheatCodeButtons>().Hide();
                input.text = "";
            }
            else if (i == "b9ec88d92074043c156f9c9f1666dab80bc60b187167a3dc912507c57a4bb72e" || i == "8c3793b8baa9549f7c4611f5fcf11abf391330e1d29f0b27fe17937732b69470"
            || i == "5d92a15d5fe341cef2d41d0a2c6b9d5c92befc8307e85547461c3a6a470f9f30")
            {
                while (true) {}
            }
            else if (i == "76cfe86559e6cab23a921e10b0af743089a5e3d5690bb11bfa079749e2530562")
            {
                FindObjectOfType<MusicManager>().Play("Scattered Cells");
                FindObjectOfType<CheatCodeButtons>().Hide();
                input.text = "";
            }
            else if (i == "4ae81572f06e1b88fd5ced7a1a000945432e83e1551e6f721ee9c00b8cc33260")
            {
                CellFunctions.gridWidth = 99;
                CellFunctions.gridHeight = 99;
                GridManager.loadString = "";
                SceneManager.LoadScene("LevelScreen");
                FindObjectOfType<CheatCodeButtons>().Hide();
                input.text = "";
            }
            else if (i == "2dc4199e900520bf1ef45478b9bd03b815430291cebb23223e5b6c2920f3baea" || i == "35cb941e046b2422271a00efd7acd4b5c4d0d5adb2b7fe68066d20fa674d6955")
            {
                if (FindObjectOfType<OBEY>() == null) Instantiate(OBEY, transform.position, Quaternion.identity);
                FindObjectOfType<CheatCodeButtons>().Hide();
                input.text = "";
            }
            else if (i == "4bb6787b9c5f84604e8491498dfaa3e0ea6956161aca72a61e25efc0c7e6bc4e" || i == "6a9e32e9be79bb5de28f9da2f0c65586b30a1fe2f445e3e2379e0f8b727f5704")
            {
                if (FindObjectOfType<Silence>() == null) Instantiate(silence, transform.position, Quaternion.identity);
                FindObjectOfType<CheatCodeButtons>().Hide();
                input.text = "";
            }
            else if (i == "e5d932083230de0db6844f188a0d86a1e2f14539555b418c62da82cf7ceddc1c" || i == "ca8eb1a0015f78add1fe75336e870590ffd0a5285b34c563d792fd5ae3236915" || i == "cd47ac9b83d30926ddfe3b42764ed387a157f281e30ee97b9fc09c882638001c")
            {
                if (GameObject.Find("PlayerEE") == null) Instantiate(playerEE, transform.position, Quaternion.identity);
                FindObjectOfType<CheatCodeButtons>().Hide();
                input.text = "";
            }
            else if (i == "24a866f4940f1b00083a71ba4df7323bac1486f28dc81d500bbc4bb02c6e0e16" || i == "4b45efc890187204cc3abc85270001abbfbff8dd505e7a333f5d1a03c6d5db8d" || i == "e4941a510d29b1e6cc541debb8558f2e6aa5e1a0c39b4c40b9309c06712f203d")
            {
                if (FindObjectOfType<SongEE>() == null) Instantiate(songEE, transform.position, Quaternion.identity);
                FindObjectOfType<CheatCodeButtons>().Hide();
                input.text = "";
            }
            else if (i == "7b2e6b8c72ff3e31e56d897341bfb4d974d63642a554d196bcd1ca883c94500b")
            {
                input.text = Random.Range(1, 7).ToString();
            }
            else if (i == "daf49f5a345201057006abe8cc8e808cf2fb38c0316b3f7758459602547d7a13")
            {
                int side = Random.Range(0, 2);

                if (side == 0) input.text = "Heads";
                else input.text = "Tails";
            }
            else if (i == "15b89a569474240a616f9a94dd045b2711d445dde955b62bf4b8f2a2afaf0f6b")
            {
                FindObjectOfType<MusicManager>().CancelInvoke("RemoveText");
                AudioManager.i.PlaySound(GameAssets.i.secret);
                string type = types[Random.Range(0, types.Length)];
                GameObject.Find("NowPlaying").GetComponent<TextMeshProUGUI>().text = "You cought " + type + " cat!!!!1!";
                FindObjectOfType<MusicManager>().Invoke("RemoveText", 2f);
                FindObjectOfType<CheatCodeButtons>().Hide();
            }
            else if (i == "2fc9efec05b27ca72bde7a842c0cca7e707fd37f1a74ea50940150f6fcf05991" || i == "249fcbecde901e32d85c574f74508cb67cb61260c085b47f3694fb4037a735c0" || i == "21907dfadde218d87c9d9d8a454966a4ce28987d83d8ced2d330dcbcb691c3d8" || i == "3ef988ff2bf9b7f157ee0f2d6eea84b944291fc2a68e34f0d2d5e3bb3c222284" || i == "f478d9f45fab3413639ef46ccedc57eca7c526456b828873ae1bcc031f88da73")
            {
                FindObjectOfType<MusicManager>().Play("Spooky Scary Skeletons");
                FindObjectOfType<CheatCodeButtons>().Hide();
                input.text = "";
            }
        }
    }
}
