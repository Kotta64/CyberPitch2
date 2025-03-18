using UnityEngine;
using TMPro;

public class RightClickMenu : MonoBehaviour
{
    [SerializeField] GameObject menuObject;
    [SerializeField] GameObject connectmenuObject;
    [SerializeField] UDPServer udp;
    [SerializeField] LogPlayer logPlayer;
    [SerializeField] TMP_InputField ipInput;
    [SerializeField] TMP_InputField portInput;


    private const string defaultIP = "127.0.0.1"; // デフォルトのIPアドレス
    private const int defaultPort = 6000; // デフォルトのポート番号

    void Start()
    {
        menuObject.SetActive(false);
        connectmenuObject.SetActive(false);
    }


    void Update()
    {
        if(!GameManager.instance.connection)
        {
            if (Input.GetMouseButtonDown(1))
            {
                menuObject.SetActive(true);
                menuObject.GetComponent<RectTransform>().position = Input.mousePosition + new Vector3(180f, -100f, 0f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                Invoke(nameof(hiddenMenu), 0.15f);
            }
        }
    }

    private void hiddenMenu()
    {
        menuObject.SetActive(false);
    }

    // デフォルの値でサーバーに接続
    public void connectButton()
    {
        udp.StartCoroutine(udp.connect(defaultIP, defaultPort));
    }

    // IPとPortを指定するウィンドウの表示
    public void connectToButton()
    {
        connectmenuObject.SetActive(true);
    }

    // rcgファイルを開くウィンドウを表示
    public void openRcgButton()
    {
        GameManager.instance.logplaying = true;
        logPlayer.StartCoroutine("startSending");
    }

    // IPとPortを指定してサーバーに接続する
    public void connectButtonWithIP()
    {
        udp.StartCoroutine(udp.connect(ipInput.text, int.Parse(portInput.text)));
        connectmenuObject.SetActive(false);
    }

    public void closeConnectUI()
    {
        connectmenuObject.SetActive(false);
    }
}
