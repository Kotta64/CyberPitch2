using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEditor;
using UnityFileBrowser;

public class LogPlayer : MonoBehaviour
{
    [SerializeField] PlayerMove pm; // プレイヤーの制御スクリプト
    [SerializeField] BallMove bm; // ボールの制御スクリプト

    public int frame; // 表示中のフレームの値
    private List<string[]> LogList; // 読み込んだログのリスト
    private bool gene; // オブジェクトを生成済みかどうか
    private string playmode; //　プレイモード
    private string lteamName; // 左チームのチーム名
    private string rteamName; // 右チームのチーム名
    private string lscore; // 左チームのスコア
    private string rscore; // 右チームのスコア
    private float speed = 0.1f; // 再生速度

    private readonly string[] PLAYMODE = {
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

    void Start() 
    {
        gene = false;
        playmode = "0";
        lteamName = "null";
        rteamName = "null";
        lscore = "0";
        rscore = "0";
    }

    // ログ再生機能の実行
    IEnumerator startSending()
    {
        if(setLog()){
            StartCoroutine(sendLog());
            GameManager.instance.connection = true;
        }
        yield return null;
    }

    // ログの読み込みと記録
    private bool setLog()
    {
        frame = 0;

        // var path = EditorUtility.OpenFilePanel("Open rcg", "", "RCG");
        var path = FileBrowser.OpenFileBrowser(new[] {"rcg"})[0];
        if (string.IsNullOrEmpty(path)) return false;

        string text = ReadText(path);
        string[] lList = text.Split('\n');

        LogList = new List<string[]>();

        string data = "";
        string[] log;
        foreach(string lg in lList)
        {
            data = lg;
            for(int i=1;i<12;i++) {
                data = data.Replace($"(l {i})", "l"+i.ToString());
                data = data.Replace($"(r {i})", "r"+i.ToString());
            }

            string buf = data.Replace(")(", " ");
            buf = buf.Replace("(", "").Replace(")", "");
            log = buf.Split(' ');

            if(log[0] == "server_param"){
                int index = Array.IndexOf(log, "nr_normal_halfs");
                GameManager.instance.max_time = int.Parse(log[index+1])*3000;
            }

            if(log[0] == "playmode") playmode = Array.IndexOf(PLAYMODE, log[2]).ToString();

            if(log[0] == "team"){
                lteamName = log[2];
                rteamName = log[3];
                lscore = log[4];
                rscore = log[5];
            }

            if(log[0] == "show"){
                string middata = $",pm,{playmode},tm,{lteamName},{rteamName},{lscore},{rscore},";
                string[] updateLog = (string.Join(",", log.Take(2))+middata+string.Join(",", log.Skip(2))).Split(',');
                LogList.Add(updateLog);
            }
        }
        return LogList.Count > 0;
    }

    // ファイルの読み込み処理
    private static string ReadText(string iPath)
    {
        using var fs = new FileStream(iPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fs, Encoding.UTF8);
        string textContent = reader.ReadToEnd();
        return textContent;
    }

    // ログを送信する処理
    IEnumerator sendLog()
    {
        while (true) {
            if(frame < LogList.Count) {
                string[] log = LogList[frame];
                if(log[0] == "show"){
                    GameManager.instance.AddLog(log);
                    if(!gene && GameManager.instance.GetLog()[0] != "D")
                    {
                        pm.StartCoroutine("GeneratePlayer");
                        bm.StartCoroutine("GenerateBall"); 
                        gene = true;
                    }
                    yield return new WaitForSeconds (speed);
                }

                GameManager.instance.communicating = true;
                frame++;
            }else{
                GameManager.instance.AddLog(GameManager.instance.GetLog(10));
                yield return new WaitForSeconds(speed);
            }
            
        }
    }
}
