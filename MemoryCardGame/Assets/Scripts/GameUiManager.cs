using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class GameUiManager : MonoBehaviour
{
    public static GameUiManager instance;
    public LoadingPanel loadingPanel;

    public GameObject gameInfoPanel;

    [Header("GamePanelProps")]
    public TMP_Text movesText;
    public TMP_Text scoreText;
    public Button homeButton;
    public GameObject gameEndPanel;
    public TMP_Text stateText;
    public TMP_Text describtionText;
    public Button GameEndHomeButton;

    public GameObject gamePanel;
    public GameObject menuPanel;


    [Header("MainMenuProps")]
    public Button playButton;
    public Button exitButton;


    [SerializeField] private GameObject blockingPanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySound("ButtonClick");
            loadingPanel.ShowLoadingPanel(() =>
            {
                ShowGamePanel();
                MemoryCardsManager.instance.PrepareGame();
            }, () =>
            {
                MemoryCardsManager.instance.ShowCards();
            });
        });

        homeButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySound("ButtonClick");
            loadingPanel.ShowLoadingPanel(() =>
            {
                ShowManuPanel();
                MemoryCardsManager.instance.SaveProgress();
            }, null);
        });


        GameEndHomeButton.onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySound("ButtonClick");
            loadingPanel.ShowLoadingPanel(() =>
            {
                ShowManuPanel();
            }, null);
        });
        
    }
    

    public void ShowManuPanel() 
    {
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        gameEndPanel.SetActive(false);
    }

    public void ShowGamePanel()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        gameEndPanel.SetActive(false);
    }

    public void UpdateScoreText(int scoreAmount) 
    {
        scoreText.text = $"Score {scoreAmount}";
    }

    public void UpdateMovesText(int movesAmount) 
    {
        movesText.text = $"Moves {movesAmount}";
    }


    public void ShowEndGamePanel(bool isWinner) 
    {
        gameEndPanel.SetActive(true);

        stateText.text = isWinner ? "Win" : "Lose";
        describtionText.text = isWinner ? "Good Job You Won This Game" : "Try Harder To Win This Game";
    }

    public void ShowBlockingPanel() 
    {
        blockingPanel.SetActive(true);
    }
    public void HideBlockingPanel() 
    {
        blockingPanel.SetActive(false);
    }
}
