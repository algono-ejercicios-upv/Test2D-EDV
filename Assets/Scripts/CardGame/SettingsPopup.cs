using UnityEngine;
using UnityEngine.UI;

public class SettingsPopup : MonoBehaviour
{
    [SerializeField] private Slider timeoutSlider;

    private TMPro.TMP_InputField inputField;
    private Slider slider;

    private void Awake()
    {
        inputField = GetComponent<TMPro.TMP_InputField>();
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        timeoutSlider.value = PlayerPrefs.GetFloat("timeout", 3f);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void SaveAndClose()
    {
        OnSubmitName(inputField.text);
        OnSpeedValue(slider.value);
        Close();
    }

    public void OnSubmitName(string name)
    {
        Debug.Log(name);
    }

    public void OnSpeedValue(float speed)
    {
        Debug.Log("Speed: " + speed);
    }

    public void OnTimeOutValue(float timeout)
    {
        Debug.Log("Timeout: " + timeout);
        PlayerPrefs.SetFloat("timeout", timeout);
    }
}
