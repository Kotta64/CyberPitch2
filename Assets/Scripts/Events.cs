using System;
using UnityEngine;
using TMPro;

public class Events : MonoBehaviour
{
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
    private const int LEFTTEAM = 7; // 左チームの得点インデックス
    private const int RIGHTTEAM = 8; // 右チームの得点インデックス
    [SerializeField] AudioClip[] goalSounds; // ゴール時のSE
    [SerializeField] AudioClip[] whistleSounds; // 審判のホイッスルSE
    [SerializeField] TextMeshProUGUI ScoreText; // 得点表示用テキスト
    private string playMode_b = ""; // 1つ前のプレイモード
    private AudioSource audioSrc; // オーディオソース

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.communicating) return;

        string[] log = GameManager.instance.GetLog();
        int playMode = getEvents(log);
        string pm = PLAYMODE[playMode];

        if(pm != playMode_b)
        {
            playMode_b = pm;
            Debug.Log(pm);

            // SE関連
            if(pm.Contains("foul") || pm.Contains("offside")) audioSrc.PlayOneShot(whistleSounds[0]);
            if(pm.Contains("before")) audioSrc.PlayOneShot(whistleSounds[1]);
            if(pm.Contains("kick") && !pm.Contains("before") || pm.Contains("ready")) audioSrc.PlayOneShot(whistleSounds[2]);

            // ゴール時の処理
            if(pm == "goal_l" || pm == "goal_r") {
                audioSrc.PlayOneShot(goalSounds[UnityEngine.Random.Range(0, goalSounds.Length)]);
                ScoreText.text = $"{log[LEFTTEAM]} : {log[RIGHTTEAM]}";
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

}
