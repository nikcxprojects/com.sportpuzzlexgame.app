using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundColorConfig", menuName = "Background Config", order = 51)]

public class BackgroundColorConfig : ScriptableObject
{
    public Color color;
    [SerializeField]
    public ColorObj[] colors;

    [Serializable]
    public struct ColorObj
    {
        public Color color;
        public string name;
    }
}
