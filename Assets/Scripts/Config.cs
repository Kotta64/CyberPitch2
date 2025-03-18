using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Config : MonoBehaviour
{
    [SerializeField] GameObject settingUI;
    [SerializeField] GameObject miniMap;
    [SerializeField] AudioSource se;

    public class ConfigData {
        public bool enableMinimap; // ミニマップの有無
        public bool enableReplay;  // リプレイの有無
        public float soundVolume;  // 音量設定(0~5)
    }

    void Start()
    {
        settingUI.SetActive(false);
        ConfigData data = new ConfigData{enableMinimap = true, enableReplay = true, soundVolume = 3f};

        if(File.Exists(Application.persistentDataPath + "/config.json")){
            string loadedJson = File.ReadAllText(Application.persistentDataPath + "/config.json");
            data = JsonUtility.FromJson<ConfigData>(loadedJson);
        }
        GameManager.instance.configData = data;
        saveConfig(data);
    }

    public void saveConfig(ConfigData data)
    {
        GameManager.instance.configData = data;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/config.json", json);

        miniMap.SetActive(data.enableMinimap);
        se.volume = data.soundVolume/5f;
    }

    public void openSetting()
    {
        settingUI.SetActive(true);
    }
}
