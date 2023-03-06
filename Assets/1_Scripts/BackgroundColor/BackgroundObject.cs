using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundObject : MonoBehaviour
{
    [SerializeField] private BackgroundColorConfig _config;
    [SerializeField] private Camera _camera;

    void Start()
    {
        if (_camera == null) _camera = GetComponent<Camera>();
        UpdateColor();
    }

    public void UpdateColor()
    {
        _camera.backgroundColor = _config.color;
    }
}
