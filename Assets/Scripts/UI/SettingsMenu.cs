using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private TextMeshProUGUI _volValueTMP;
    [SerializeField] private TextMeshProUGUI _volSfxValueTMP;
    [SerializeField] private TextMeshProUGUI _volMusicValueTMP;

    private Slider _volSlider;
    private Slider _sfxSlider;
    private Slider _musicSlider;

    void Start()
    {
        _volSlider = _volValueTMP.GetComponentInParent<Slider>();
        _sfxSlider = _volSfxValueTMP.GetComponentInParent<Slider>();
        _musicSlider = _volMusicValueTMP.GetComponentInParent<Slider>();

        _audioMixer.GetFloat("MasterVol", out float vol);
        _audioMixer.GetFloat("SfxVol", out float sfx);
        _audioMixer.GetFloat("MusicVol", out float music);

        _volSlider.value = DbtoVol(vol);
        _sfxSlider.value = DbtoVol(sfx);
        _musicSlider.value = DbtoVol(music);

    }

    public void OnChangeVolume(float newVolume)
    {
        _volValueTMP.text = newVolume.ToString();
        Debug.Log(_audioMixer.SetFloat("MasterVol", VolToDb(newVolume)));
    }

    public void OnChangeSfxVolume(float newVolume)
    {
        _volSfxValueTMP.text = newVolume.ToString();
        _audioMixer.SetFloat("SfxVol", VolToDb(newVolume));
    }

    public void OnChangeMusicVolume(float newVolume)
    {
        _volMusicValueTMP.text = newVolume.ToString();
        _audioMixer.SetFloat("MusicVol", VolToDb(newVolume));
    }

    private static float VolToDb(float volume)
    {
        if (volume == 0) return -80;
        return Mathf.Log(volume * 0.01f) * 40;
    }

    private static int DbtoVol(float decibal)
    {
        if (decibal == -80) return 0;
        return 100 * (int)Mathf.Pow(10, decibal / 40);
    }
}
