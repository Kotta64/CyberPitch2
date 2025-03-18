using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public int max_time; //試合時間(default:6000)
    public bool communicating; //rcssserverと通信中かどうか
    public bool connection; //接続試験中かどうか
    public bool replaying; //リプレイ再生中かどうか
    public bool logplaying;

    private List<string[]> LogList; //ログを格納
    private List<string[]> ReplayList; //リプレイ用のログを格納
    private int logNum; //ログの番号
    private const int LOGMAXSIZE = 10;
    private const int REPLAYMAXSIZE = 50;

    public Config.ConfigData configData = new Config.ConfigData{enableMinimap = true, enableReplay = true, soundVolume = -1.0f}; // configデータ

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        resetParam();
        Application.targetFrameRate = 60;
    }

    // パラメーターのリセット
    public void resetParam()
    {
        max_time = -1;
        communicating = false;
        connection = false;
        replaying = false;
        LogList = new List<string[]>();
        ReplayList = new List<string[]>();
        logNum = 0;
        logplaying = false;
    }

    // ログデータの追加
    public void AddLog(string[] lg)
    {
        Array.Resize(ref lg, lg.Length + 1);
        lg[lg.Length - 1] = logNum.ToString();
        LogList.Add(lg);
        logNum++;
        if(LogList.Count > LOGMAXSIZE && !replaying) {
            AddReplay(LogList[0]);
            LogList.RemoveAt(0);
        }
    }

    // ログデータの取得
    public string[] GetLog(int index = 0)
    {
        if(LogList.Count >= LOGMAXSIZE) return LogList[Mathf.Clamp(index, 0, LOGMAXSIZE-1)];
        else return new string[] {"D", "0"};
    }

    // リプレイデータの追加
    private void AddReplay(string[] lg)
    {
        ReplayList.Add(lg);
        if(ReplayList.Count > REPLAYMAXSIZE) ReplayList.RemoveAt(0);
    }
}
