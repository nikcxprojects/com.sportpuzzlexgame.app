using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text TimerText; 
    private float timer;

    private bool pause = false;
    void Update()
    {
        if (pause) return;
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer % 60F);
        TimerText.text = minutes.ToString ("00") + ":" + seconds.ToString ("00");
    }

    public void Restart()
    {
        Pause(false);
        timer = 0;
    }

    public void Pause(bool val)
    {
        pause = val;
    }
    
    public string GetText()
    {
        return TimerText.text;
    }
}
