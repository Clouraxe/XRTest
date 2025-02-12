using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _volValueTMP;
    
    public void OnChangeVolume(float newVolume)
    {
        _volValueTMP.text = ((int)newVolume).ToString();
        AudioListener.volume = newVolume / 100.0f;
    }
}
