using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class GameUiManager : MonoBehaviour
{
    public static GameUiManager instance;


    public GameObject gameInfoPanel;
    public TMP_Text movesText;
    public TMP_Text scoreText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateScoreText(int scoreAmount) 
    {
        movesText.text = $"Score {scoreAmount}";
    }

    public void UpdateMovesText(int movesAmount) 
    {
        movesText.text = $"Moves {movesAmount}";
    }
}
