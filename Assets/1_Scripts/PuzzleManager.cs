using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private RuntimeGeneration _gen;
    [SerializeField] private PuzzleConfig _config;

    [SerializeField] private GameObject _finishGameUI;
    [SerializeField] private Text _finishTimeText;
    [SerializeField] private Timer _timer;

    private bool _complete;

    private void Start()
    {
        _gen.puzzleGenerator.cols = _config.cols;
        _gen.puzzleGenerator.rows = _config.rows;
        _gen.puzzleGenerator.imageScale = _config.imageSize;
        _gen.image = _config.image;
        _finishGameUI.SetActive(false);
        StartCoroutine(_gen.GeneratePuzzle());
    }

    private void Update()
    {
        if (_gen.puzzleGenerator.puzzle != null && _gen.puzzleGenerator.puzzle.IsAssembled() && !_complete)
        {
            _complete = true;
            FinishGame();
        }
    }

    private void FinishGame()
    {
        _finishGameUI.SetActive(true);
        _timer.Pause(true);
        var s = $"Congratulations! \n Puzzle completed in \n {_timer.TimerText.text}";
        Vibration.Vibrate(700);
        _finishTimeText.text = s;
    }
}