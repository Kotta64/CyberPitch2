using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lteamName;
    [SerializeField] TextMeshProUGUI rteamName;
    [SerializeField] TextMeshProUGUI score;
    [SerializeField] TextMeshProUGUI timer;
    

    private const int LEFTTEAM = 5;
    private const int RIGHTTEAM = 6;

    void Start()
    {
        StartCoroutine("setlteamName");
        StartCoroutine("setrteamName");
        StartCoroutine("setScore");
        StartCoroutine("setTimer");
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

    IEnumerator setTimer()
    {
        int max = -1, now;
        while(true)
        {
            string[] log = GameManager.instance.GetLog();
            if(max == -1) max = GameManager.instance.max_time;
            now = int.Parse(log[1]);

            if(max - now < 0) {
                max += 1000;
                timer.color = Color.red;
            }

            if(log[0] != "D") timer.text = $"{(max-now)/10}s";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
