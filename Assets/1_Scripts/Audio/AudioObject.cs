using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{

    private float time = 0;

    public AudioType type;

    private void OnEnable()
    {
        switch (type)
        {
            case AudioType.Music:
                GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("VolumeMusic", 1);
                break;
            case AudioType.Sounds:
                GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("VolumeSounds", 1);
                break;
        }

        time = GetComponent<AudioSource>().clip.length;
    }

    public void Init(float time)
    {
        StopAllCoroutines();
        this.time = time;
        Destroy();
    }

    public void Destroy()
    {
        StartCoroutine(DestroyGameObject());
    }
    
    private IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    public enum AudioType
    {
        Sounds,
        Music
    }
}