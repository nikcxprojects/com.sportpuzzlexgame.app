using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Game Config", order = 50)]
public class PuzzleConfig : ScriptableObject
{
    public Texture2D image;
    public int rows;
    public int cols;
    public float imageSize;
}
