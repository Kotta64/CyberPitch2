using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] GameObject lp_prefab; // 左チームのプレファブ
    [SerializeField] GameObject lg_prefab; // 右チームのプレファブ
    [SerializeField] GameObject rp_prefab; // 左チームキーパーのプレファブ
    [SerializeField] GameObject rg_prefab; // 右チームキーパーのプレファブ
    [SerializeField] GameObject lp_object; // 左チームの親オブジェクト
    [SerializeField] GameObject rp_object; // 右チームの親オブジェクト
    [SerializeField] CameraSwitch cmsw; // カメラ切り替えスクリプト
    [SerializeField] GameObject mainUI;
    [SerializeField] GameObject lgoal;
    [SerializeField] GameObject rgoal;
    [SerializeField] AudioSource audioSource;

    private List<GameObject> lplayers = new List<GameObject>(); // 左チームのプレイヤーオブジェクト
    private List<GameObject> rplayers = new List<GameObject>(); // 左チームのプレイヤーオブジェクト

    private List<Vector3> lp_position_b; // 左チームの移動前座標
    private List<Vector3> lp_position_a; // 右チームの移動前座標
    private List<Vector3> rp_position_b; // 左チームの移動後座標
    private List<Vector3> rp_position_a; // 右チームの移動後座標

    private List<float> lp_rotation_b; // 左チームの移動前角度
    private List<float> lp_rotation_a; // 右チームの移動前角度
    private List<float> rp_rotation_b; // 左チームの移動後角度
    private List<float> rp_rotation_a; // 右チームの移動後角度

    private int logNumB;
    private int progress; // 補間値

    private const int xIndex = 3;
    private const int yIndex = 4;
    private const int rIndex = 7;

    private void Start()
    {
        mainUI.SetActive(false);
        lgoal.SetActive(false);
        rgoal.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(lplayers.Count < 11) return;
        
        string[] log = GameManager.instance.GetLog();
        int lastNum = int.Parse(log[log.Length - 1]);

        if(logNumB != lastNum){
            logNumB = lastNum;
            progress = 0;

            lp_position_b = new List<Vector3>(lp_position_a);
            rp_position_b = new List<Vector3>(rp_position_a);

            lp_rotation_b = new List<float>(lp_rotation_a);
            rp_rotation_b = new List<float>(rp_rotation_a);

            lp_position_a = new List<Vector3>();
            rp_position_a = new List<Vector3>();

            lp_rotation_a = new List<float>();
            rp_rotation_a = new List<float>();

            for(int i=0; i<11; i++) {
                var l_data = getPlayerData(log, $"l{i+1}");
                var r_data = getPlayerData(log, $"r{i+1}");

                lp_position_a.Add(l_data.position);
                rp_position_a.Add(r_data.position);

                lp_rotation_a.Add(l_data.angle);
                rp_rotation_a.Add(r_data.angle);
            }
        }

        for(int i=0; i<11; i++) {
            lplayers[i].transform.position = Vector3.Lerp(lp_position_b[i], lp_position_a[i], progress/5.0f);
            rplayers[i].transform.position = Vector3.Lerp(rp_position_b[i], rp_position_a[i], progress/5.0f);

            lplayers[i].transform.rotation = Quaternion.Euler(0, (lp_rotation_a[i] - lp_rotation_b[i]) * progress/5.0f + lp_rotation_b[i], 0);
            rplayers[i].transform.rotation = Quaternion.Euler(0, (rp_rotation_a[i] - rp_rotation_b[i]) * progress/5.0f + rp_rotation_b[i], 0);
        }
        progress++;
    }

    // プレイヤーの生成
    public IEnumerator GeneratePlayer()
    {
        lp_position_a = new List<Vector3>();
        rp_position_a = new List<Vector3>();
        
        lp_rotation_a = new List<float>();
        rp_rotation_a = new List<float>();

        if(lplayers.Count == 0)
        {
            Vector3 p;
            do{
                p = getPlayerData(GameManager.instance.GetLog(), "l1").position;
                yield return new WaitForSeconds(0.1f);
            } while(p.y == -100f);


            var log = GameManager.instance.GetLog();
            for(int i=0; i<11; i++) {
                var l_data = getPlayerData(log, $"l{i+1}");
                var r_data = getPlayerData(log, $"r{i+1}");

                if(i==0) {
                    lplayers.Add(Instantiate(lg_prefab, l_data.position, Quaternion.Euler(0, l_data.angle, 0)));
                    rplayers.Add(Instantiate(rg_prefab, r_data.position, Quaternion.Euler(0, r_data.angle, 0)));
                }else{
                    lplayers.Add(Instantiate(lp_prefab, l_data.position, Quaternion.Euler(0, l_data.angle, 0)));
                    rplayers.Add(Instantiate(rp_prefab, r_data.position, Quaternion.Euler(0, r_data.angle, 0)));
                }

                lplayers[i].transform.parent = lp_object.transform;
                rplayers[i].transform.parent = rp_object.transform;

                lplayers[i].transform.Find("Id").gameObject.GetComponent<TextMeshPro>().text = (i+1).ToString();
                rplayers[i].transform.Find("Id").gameObject.GetComponent<TextMeshPro>().text = (i+1).ToString();

                lp_position_a.Add(l_data.position);
                rp_position_a.Add(r_data.position);

                lp_rotation_a.Add(l_data.angle);
                rp_rotation_a.Add(r_data.angle);
            }
        }
        setGame();
    }

    // 試合開始の準備処理
    private void setGame()
    {
        cmsw.GetComponent<Button>().interactable = true;
        cmsw.OnClicked();
        mainUI.SetActive(true);
        lgoal.SetActive(true);
        rgoal.SetActive(true);
        audioSource.Stop();
        audioSource.Play();
    }

    // プレイヤーのの座標取得
    private (Vector3 position, float angle) getPlayerData(string[] log, string key)
    {
        int index = Array.IndexOf(log, key);
        if(index != -1) return (new Vector3(float.Parse(log[index+xIndex]), 0.0f, -float.Parse(log[index+yIndex])), float.Parse(log[index+rIndex]));
        return (new Vector3(0f, -100f, 0f), 0f);
    }
}
