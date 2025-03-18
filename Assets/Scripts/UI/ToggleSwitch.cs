using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] private SettingUI settingUI;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private RectTransform handle;
    private bool isOn = true;
    private float handlePosX;
    private static readonly Color OFF_BG_COLOR = new Color(0.78f, 0.78f, 0.78f);
    private static readonly Color ON_BG_COLOR = new Color(0.2f, 0.84f, 0.3f);

    void Start()
    {
        handlePosX = Mathf.Abs(handle.anchoredPosition.x);
    }

    public void setValue(bool value)
    {
        isOn = !value;
        Toggle();
    }

    public bool getValue()
    {
        return isOn;
    }

    public void Toggle()
    {
        isOn = !isOn;
        var bgColor = isOn ? ON_BG_COLOR : OFF_BG_COLOR;
        var handleDestX = isOn ? handlePosX : -handlePosX;
        backgroundImage.color = bgColor;
        handle.anchoredPosition = new Vector3(handleDestX, 0f, 0f);

        settingUI.changedConfig();
    }
}
