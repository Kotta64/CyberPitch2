using System;
using System.Collections;
using UnityEngine;
using Unity.Cinemachine;


public class BallMove : MonoBehaviour
{
    [SerializeField] GameObject ball_prefab; //ボールのプレファブ
    [SerializeField] GameObject ball_m; //ボールの親オブジェクト
    [SerializeField] AudioSource bgm;
    public CinemachineCamera[] virtualCamera;
    private Vector3 ball_position_b; //ボールの移動前座標
    private Vector3 ball_position_a; //ボールの移動後座標
    private GameObject ball; //ボールオブジェクト
    private int logNumB;
    private int progress; // 補間値

    private const int xIndex = 1;
    private const int yIndex = 2;
    
    private void FixedUpdate()
    {
        if(ball == null) return;
        
        string[] log = GameManager.instance.GetLog();
        
        var ball_data = getBallData(log);
        int lastNum = int.Parse(log[log.Length - 1]);
        if(logNumB != lastNum){
            logNumB = lastNum;
            progress = 0;
            ball_position_b = ball_position_a;
            ball_position_a = ball_data;
        }
        ball.transform.position = Vector3.Lerp(ball_position_b, ball_position_a, progress/5.0f);
        ball.transform.rotation = Quaternion.Euler(0, Mathf.Atan2(ball.transform.position.x, ball.transform.position.z)*Mathf.Rad2Deg, 0);
        progress++;

        bgm.volume = GameManager.instance.configData.soundVolume/5f*(0.5f+Mathf.Abs(ball.transform.position.x/100f));
    }

    // ボールの生成
    public IEnumerator GenerateBall()
    {
        if(ball == null)
        {
            Vector3 p;
            do{
                p = getBallData(GameManager.instance.GetLog());
                yield return new WaitForSeconds(0.1f);
            } while(p.y == -100f);
            ball = Instantiate(ball_prefab, p, Quaternion.identity);
            ball.transform.parent = ball_m.transform;
            ball_position_a = p;

            for(int i=0; i < virtualCamera.Length; i++) {
                virtualCamera[i].Follow = ball.transform;
                virtualCamera[i].LookAt = ball.transform;
            }
        }
    }

    // ボールの座標取得
    private Vector3 getBallData(string[] log)
    {
        int index = Array.IndexOf(log, "b");
        if(index != -1) return new Vector3(float.Parse(log[index+xIndex]), 0.0f, -float.Parse(log[index+yIndex]));
        else return new Vector3(0f, -100f, 0f);
    }
}
