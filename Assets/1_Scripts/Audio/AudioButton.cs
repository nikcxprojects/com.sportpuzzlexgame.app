using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour
{
    private int volumeSounds;
    private int vibration;
    [SerializeField] private Text text;
    [SerializeField] private Text text3;
    
    void Start()
    {
        volumeSounds = PlayerPrefs.GetInt("VolumeSounds", 1);
        UpdateUI();
    }

    public void ChangeSoundsVolume()
    {
        volumeSounds = volumeSounds == 0 ? 1 : 0;
        PlayerPrefs.SetInt("VolumeSounds", volumeSounds);
        UpdateUI();
    }
    
    public void ChangeVipration()
    {
        vibration = vibration == 0 ? 1 : 0;
        PlayerPrefs.SetInt("Vibration", vibration);
        UpdateUI();
    }

    private void UpdateUI()
    {
        text.text = volumeSounds == 0 ? "SOUNDS OFF" : "SOUNDS ON";
        text3.text = vibration == 0 ? "VIBRA OFF" : "VIBRA ON";
    }
    
}
