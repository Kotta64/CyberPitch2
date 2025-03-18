using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections;

public class UDPServer : MonoBehaviour
{
    private UdpClient udpClient;
    private string host; // 接続先のIPアドレス
    private static int updatedPORT; // 変更後のポート番号
    [SerializeField] BallMove bm; // ボール制御プログラム
    [SerializeField] PlayerMove pm; // プレイヤー制御プログラム

    private void Start() 
    {
        udpClient = new UdpClient();
        udpClient.Client.ReceiveTimeout = 500;
        updatedPORT = 0;
    }

    // rcssserverに接続
    public IEnumerator connect(string ip, int port)
    {
        if(!(GameManager.instance.communicating)) {
            host = ip;
            var message = Encoding.UTF8.GetBytes("(dispinit version 5)");
            try{
                IPEndPoint Ep = new IPEndPoint(IPAddress.Parse(host), port);
                udpClient.Send(message, message.Length, Ep);
                udpClient.BeginReceive(UDPReceive, udpClient);
            }catch(Exception e){
                Debug.Log(e);
            }
        }
        GameManager.instance.connection = true;

        yield return new WaitForSeconds(0.5f);

        if(!(GameManager.instance.communicating)) {
            udpClient.Close();
            udpClient = new UdpClient();
            udpClient.Client.ReceiveTimeout = 500;
            GameManager.instance.connection = false;
            Debug.Log("Connection Failed");
        }else{
            pm.StartCoroutine("GeneratePlayer");
            bm.StartCoroutine("GenerateBall");
            Debug.Log("Connection Success");
        }
    }

    // キックオフの指示
    public void Kickoff()
    {   
        if(GameManager.instance.logplaying) {
            return;
        }
        if(updatedPORT != 0) {
            var message = Encoding.UTF8.GetBytes("(dispstart)");
            IPEndPoint Ep = new IPEndPoint(IPAddress.Parse(host), updatedPORT);
            try{
                udpClient.Send(message, message.Length, Ep);
            }catch(Exception e){
                Debug.Log(e);
            }
        }
    }

    // データの受信処理
    private static void UDPReceive(IAsyncResult res)
    {
        UdpClient getUdp = (UdpClient)res.AsyncState;
        IPEndPoint ipEnd = null;
        byte[] getByte = getUdp.EndReceive(res, ref ipEnd);
        
        if(ipEnd.Port != updatedPORT) {
            updatedPORT = ipEnd.Port;
            Debug.Log($"Updated Server Port to {ipEnd.Port}");
        }

        string text = Encoding.UTF8.GetString(getByte);

        for(int i=1;i<12;i++) {
            text = text.Replace($"(l {i})", "l"+i.ToString());
            text = text.Replace($"(r {i})", "r"+i.ToString());
        }
        text = text.Replace(")(", " ");
        text = text.Replace("(", "").Replace(")", "");

        string[] log = text.Split(' ');

        if(log[0] == "server_param"){
            int index = Array.IndexOf(log, "nr_normal_halfs");
            GameManager.instance.max_time = int.Parse(log[index+1])*3000;
        }

        if(log[0] == "show") GameManager.instance.AddLog(log);

        // Debug.Log(string.Join(",", log));
        GameManager.instance.communicating = true;

        getUdp.BeginReceive(UDPReceive, getUdp); 
    }

    public void Disconnect() 
    {
        udpClient.Close();
    }
}
