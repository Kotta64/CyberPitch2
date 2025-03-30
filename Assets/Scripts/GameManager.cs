using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public readonly string[] PLAYMODE = {
        "",                   
        "before_kick_off",                  
        "time_over",                        
        "play_on",                          
        "kick_off_l",                       
        "kick_off_r",                       
        "kick_in_l",                        
        "kick_in_r",                        
        "free_kick_l",                      
        "free_kick_r",                      
        "corner_kick_l",                    
        "corner_kick_r",                    
        "goal_kick_l",                      
        "goal_kick_r",                      
        "goal_l",                           
        "goal_r",                           
        "drop_ball",                        
        "offside_l",                        
        "offside_r",                        
        "penalty_kick_l",                   
        "penalty_kick_r",                   
        "first_half_over",                  
        "pause",                            
        "human_judge",                      
        "foul_charge_l",                    
        "foul_charge_r",                    
        "foul_push_l",                      
        "foul_push_r",                      
        "foul_multiple_attack_l",           
        "foul_multiple_attack_r",           
        "foul_ballout_l",                   
        "foul_ballout_r",                   
        "back_pass_l",                      
        "back_pass_r",                      
        "free_kick_fault_l",                
        "free_kick_fault_r",                
        "catch_fault_l",                    
        "catch_fault_r",                    
        "indirect_free_kick_l",             
        "indirect_free_kick_r",             
        "penalty_setup_l",                  
        "penalty_setup_r",                  
        "penalty_ready_l",                  
        "penalty_ready_r",                  
        "penalty_taken_l",                  
        "penalty_taken_r",                  
        "penalty_miss_l",                   
        "penalty_miss_r",                   
        "penalty_score_l",                  
        "penalty_score_r",                  
        "illegal_defense_l",                
        "illegal_defense_r"                 
    };
    [SerializeField] UDPServer udp;

    public static GameManager instance = null;
    public int max_time; //試合時間(default:6000)
    public bool communicating; //rcssserverと通信中かどうか
    public bool connection; //接続試験中かどうか
    public bool replaying; //リプレイ再生中かどうか
    public bool waitreplaying; //リプレイ再生待機中かどうか
    public bool logplaying; //ログ再生中かどうか

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

    private void Start()
    {
        CMLine();
    }

    // コマンドライン引数の処理
    private void CMLine()
    {
        var args = System.Environment.GetCommandLineArgs();
        string pattern = @"\d{1,4}\.\d{1,4}\.\d{1,4}\.\d{1,4}";
        foreach (var arg in args)
        {
            Match match = Regex.Match(arg, pattern);
            if(match.Success){
                udp.StartCoroutine(udp.connect(match.Value, 6000));
            }
            // if(arg == "--auto" || arg == "-a") GameManager.instance.automode = true;
            // if(arg == "--skip" || arg == "-s") GameManager.instance.skipmode = true;
        }
    }

    // パラメーターのリセット
    public void resetParam()
    {
        max_time = -1;
        communicating = false;
        connection = false;
        replaying = false;
        waitreplaying = false;
        LogList = new List<string[]>();
        ReplayList = new List<string[]>();
        logNum = 1;
        logplaying = false;
    }

    // ログデータの追加
    public void AddLog(string[] lg)
    {
        if(waitreplaying) return;

        Array.Resize(ref lg, lg.Length + 1);
        lg[lg.Length - 1] = logNum.ToString();
        LogList.Add(lg);
        logNum++;

        //if(LogList.Count > LOGMAXSIZE && !replaying) {
        if(LogList.Count > LOGMAXSIZE) {
            AddReplay(LogList[0]);
            LogList.RemoveAt(0);
        }
    }

    // ログデータの取得
    public string[] GetLog(int index = 0)
    {
        if(index < 0 && LogList.Count >= LOGMAXSIZE) return LogList[LogList.Count+index];
        else if(LogList.Count >= LOGMAXSIZE) return LogList[Mathf.Clamp(index, 0, LOGMAXSIZE-1)];
        else return new string[] {"D", "0"};
    }

    // リプレイデータの追加
    private void AddReplay(string[] lg)
    {
        if(!replaying){
            ReplayList.Add(lg);
            if(ReplayList.Count > REPLAYMAXSIZE) ReplayList.RemoveAt(0);
        }
    }

    // リプレイデータのサイズが一定以上かどうか取得
    public bool GetReplaySize()
    {
        return ReplayList.Count >= REPLAYMAXSIZE && configData.enableReplay;
    }


    // リプレイデータの生成
    public void GenerateRepalyData()
    {
        ReplayList.Add(GetLog());
        LogList = ReplayList;
        ReplayList = new List<string[]>();
    }
}
