using UnityEngine;
using TMPro;
using System.Collections;

public class MainUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lteamName;
    [SerializeField] TextMeshProUGUI rteamName;
    [SerializeField] TextMeshProUGUI score;

    private const int LEFTTEAM = 5;
    private const int RIGHTTEAM = 6;

    void Start()
    {
        StartCoroutine("setlteamName");
        StartCoroutine("setrteamName");
        StartCoroutine("setScore");
    }

    IEnumerator setlteamName()
    {
        while(lteamName.text == "null")
        {
            string[] log = GameManager.instance.GetLog();
            if(log[0] != "D") lteamName.text = log[LEFTTEAM];
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator setrteamName()
    {
        while(rteamName.text == "null")
        {
            string[] log = GameManager.instance.GetLog();
            if(log[0] != "D") rteamName.text = log[RIGHTTEAM];
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator setScore()
    {
        while(score.text == "null")
        {
            string[] log = GameManager.instance.GetLog();
            if(log[0] != "D") score.text = $"{log[LEFTTEAM+2]} : {log[RIGHTTEAM+2]}";
            yield return new WaitForSeconds(0.1f);
        }
    }
}
