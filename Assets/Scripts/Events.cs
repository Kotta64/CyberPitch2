using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class Events : MonoBehaviour
{
    private const int LEFTTEAM = 7; // 左チームの得点インデックス
    private const int RIGHTTEAM = 8; // 右チームの得点インデックス
    [SerializeField] AudioClip[] goalSounds; // ゴール時のSE
    [SerializeField] AudioClip[] whistleSounds; // 審判のホイッスルSE
    [SerializeField] GameObject mainUI; // メインUI
    [SerializeField] GameObject goalEffectPrefab;
    [SerializeField] GameObject clockBackPrefab;
    [SerializeField] GameObject clockGoPrefab;
    [SerializeField] TextMeshProUGUI ScoreText; // 得点表示用テキスト
    [SerializeField] CameraSwitch cmsw; // カメラ切り替えスクリプト
    [SerializeField] GameObject penalty; // PK用オブジェクト
    [SerializeField] TextMeshProUGUI penalty_l; // PK左チーム用テキスト
    [SerializeField] TextMeshProUGUI penalty_r; // PK右チーム用テキスト
    [SerializeField] GameObject playtime; // 試合時間表示用テキスト
    
    private string playMode_b = ""; // 1つ前のプレイモード
    private AudioSource audioSrc; // オーディオソース
    private GameObject goalEffect; // ゴール演出用オブジェクト
    private GameObject clockBack;
    private GameObject clockGo;
    
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        penalty.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.communicating) return;

        string[] log = GameManager.instance.GetLog();
        int playMode = getEvents(log);
        string pm = GameManager.instance.PLAYMODE[playMode];

        if(pm != playMode_b)
        {
            playMode_b = pm;
            //Debug.Log(pm);

            // SE関連
            if(pm.Contains("foul") || pm.Contains("offside")) audioSrc.PlayOneShot(whistleSounds[0]);
            if(pm.Contains("before")) audioSrc.PlayOneShot(whistleSounds[1]);
            if(pm.Contains("kick") && !pm.Contains("before") || pm.Contains("ready")) audioSrc.PlayOneShot(whistleSounds[2]);

            // ゴール時の処理
            if(pm == "goal_l" || pm == "goal_r") {
                audioSrc.PlayOneShot(goalSounds[UnityEngine.Random.Range(0, goalSounds.Length)]);
                ScoreText.text = $"{log[LEFTTEAM]} : {log[RIGHTTEAM]}";
                if(goalEffect == null) goalEffect = Instantiate(goalEffectPrefab, mainUI.transform);
                else goalEffect.SetActive(true);

                StartCoroutine(StartReplay((pm == "goal_l") ? 1 : 0));
            }

            // PK戦の処理
            if(pm.Contains("penalty")){
                penalty.SetActive(true);
                playtime.SetActive(false);
                switch (pm)
                {
                    case "penalty_score_l":
                        penalty_l.text += "O|";
                        break;
                    case "penalty_score_r":
                        penalty_r.text += "O|";
                        break;
                    case "penalty_miss_l":
                        penalty_l.text += "X|";
                        break;
                    case "penalty_miss_r":
                        penalty_r.text += "X|";
                        break;
                    default:
                        break;
                }
            }

            // 試合終了時の処理
            if(pm == "time_over"){
                Debug.Log("FINISH!!");
                GameManager.instance.communicating = false;
            }
        }
    }

    // イベントの検出
    private int getEvents(string[] log)
    {
        int index = Array.IndexOf(log, "pm");
        if(index == -1) return 0;
        return int.Parse(log[index+1]);
    }

    // リプレイ映像の再生処理
    IEnumerator StartReplay(int lr)
    {
        if(GameManager.instance.GetReplaySize()){
            int count = int.Parse(GameManager.instance.GetLog()[1]);
            GameManager.instance.replaying = true;
            GameManager.instance.waitreplaying = true;
            // Debug.Log(count);

            yield return new WaitForSeconds (1.5f);

            if(clockBack == null) clockBack = Instantiate(clockBackPrefab, mainUI.transform);
            else clockBack.SetActive(true);

            yield return new WaitForSeconds (1.5f);

            GameManager.instance.GenerateRepalyData();
            GameManager.instance.waitreplaying = false;
            cmsw.change2GoalCamera(lr);

            while(true){
                int now = int.Parse(GameManager.instance.GetLog()[1]);
                if(now >= count) break;

                yield return new WaitForSeconds (0.05f);
            }

            GameManager.instance.waitreplaying = true;

            yield return new WaitForSeconds (0.3f);

            if(clockGo == null) clockGo = Instantiate(clockGoPrefab, mainUI.transform);
            else clockGo.SetActive(true);

            yield return new WaitForSeconds (1.3f);

            GameManager.instance.waitreplaying = false;
            cmsw.change2defaultCamera();
            
            yield return new WaitForSeconds (1.5f);
            
            clockBack.SetActive(false);
            clockGo.SetActive(false);
            GameManager.instance.replaying = false;
            cmsw.changeBlendTime2default();

        }else{
            yield return new WaitForSeconds (2.5f);
        }

        goalEffect.SetActive(false);
        yield return null;
    }
}
