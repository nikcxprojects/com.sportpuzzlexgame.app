using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorManager : MonoBehaviour
{
    [SerializeField] private BackgroundColorConfig _config;
    [SerializeField] private Text _text;
    private int _currentColor;

    private void Awake()
    {
        Application.targetFrameRate = 90;
        _currentColor = PlayerPrefs.GetInt("Color");
        SwitchColor(_currentColor);
    }
    
    public void SwitchColor()
    {
        if (_config.colors.Length-1 <= _currentColor)
            _currentColor = 0;
        else
            _currentColor++;

        Debug.Log(_config.colors.Length);
        SwitchColor(_currentColor);
        
        var objs = FindObjectsOfType<BackgroundObject>();
        foreach (var obj in objs) obj.UpdateColor();
    }

    private void SwitchColor(int i)
    {
        PlayerPrefs.SetInt("Color", i);
        _config.color = _config.colors[i].color;
        _text.text = "Background: " + _config.colors[i].name;
    }
}
