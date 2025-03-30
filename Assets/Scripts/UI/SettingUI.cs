using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SettingUI : MonoBehaviour
{
    [SerializeField] ToggleSwitch minimapButton;
    [SerializeField] ToggleSwitch replayButton;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Config cc;
    [SerializeField] UDPServer udp;

    private void OnEnable() 
    {
        StartCoroutine("reloadData");
    }

    private IEnumerator reloadData()
    {
        yield return new WaitForSeconds(0.1f);
        Config.ConfigData config;
        do
        {
            config = GameManager.instance.configData;
            yield return new WaitForSeconds(0.1f); 
        } while(config.soundVolume < 0f);

        minimapButton.setValue(config.enableMinimap);
        replayButton.setValue(config.enableReplay);
        volumeSlider.value = config.soundVolume;
    }

    public void changedConfig()
    {
        Config.ConfigData data = new Config.ConfigData{
            enableMinimap = minimapButton.getValue(),
            enableReplay = replayButton.getValue(),
            soundVolume = volumeSlider.value
        };
        cc.saveConfig(data);
    }

    public void quit()
    {
        gameObject.SetActive(false);
    }

    public void Reset()
    {
        udp.Disconnect();
        GameManager.instance.resetParam();
        SceneManager.LoadScene("MainScene");
    }
}
