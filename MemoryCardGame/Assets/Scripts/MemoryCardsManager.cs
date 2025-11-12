using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MemoryCardsManager : MonoBehaviour
{

    public static MemoryCardsManager instance;

    public int totalCardsCouple = 8;
    private Dictionary<int, MemoryCard> memoryCardsDictionary;
    private List<MemoryCard> memoryCards;
    public float cardsViewDuration = 4;
    public MemoryCard memoryCardPrefab;
    public GridLayoutGroup gridLayout;
    public List<Sprite> cardSprites;

    public int maxWrnogMovementAmount = 5;
    public int wrongMoveAmount;
    public int pendingMoves = 0;
    private MemoryCard card1;
    private MemoryCard card2;


    private void Awake()
    {
        instance = this;
    }





    public void PrepareGame() 
    {
        InitCards();
        GameUiManager.instance.UpdateMovesText(wrongMoveAmount);
        GameUiManager.instance.UpdateScoreText(0);
    }

    private void InitCards()
    {
        InitCardsSize();
        wrongMoveAmount = maxWrnogMovementAmount;
        pendingMoves = 0;
        Score = 0;

        if (memoryCards == null)
            CreateCards();


        if (ProgressManager.IsThereSavedData())
        {
            LoadLevel();
        }
        else
        {

            foreach (MemoryCard card in memoryCards)
            {
                card.ForceHide();
            }

            ShuffleCards(memoryCards);
        }



        memoryCardsDictionary = new Dictionary<int, MemoryCard>();

        foreach (MemoryCard card in memoryCards) 
        {
            memoryCardsDictionary.Add(card.GetId(), card);
        }
        foreach (MemoryCard card in memoryCards)
        {
            card.transform.SetParent(gridLayout.transform);
            card.transform.localScale = Vector3.one;
        }
    }
    private void CreateCards()
    {
        memoryCards = new List<MemoryCard>();
        for (int i = 0; i < totalCardsCouple * 2; i += 2)
        {
            MemoryCard newCard1 = Instantiate(memoryCardPrefab);
            MemoryCard newCard2 = Instantiate(memoryCardPrefab);

            int radomCardSpriteIndex = UnityEngine.Random.Range(0, cardSprites.Count);
            newCard1.InitCard(i, cardSprites[radomCardSpriteIndex], this);
            newCard2.InitCard(i + 1, cardSprites[radomCardSpriteIndex], this);



            memoryCards.Add(newCard1);
            memoryCards.Add(newCard2);

        }
    }

    private void ShuffleCards<T>(List<T> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int rand = Random.Range(i, cards.Count);
            (cards[i], cards[rand]) = (cards[rand], cards[i]);
        }
    }

    private void InitCardsSize()
    {
        RectTransform rect = gridLayout.GetComponent<RectTransform>();
        float rectX = rect.rect.width;
        float rectY = rect.rect.height;

        int totalCards = totalCardsCouple * 2;

        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(totalCards));

        float spacingRatio = 0.025f;

        float spacing = rectX * spacingRatio;

        float cellWidth = (rectX - (spacing * (gridSize - 1))) / gridSize;
        float cellHeight = (rectY - (spacing * (gridSize - 1))) / gridSize;
        float cellSize = Mathf.Min(cellWidth * 0.9f, cellHeight * 0.9f);

        gridLayout.constraint = UnityEngine.UI.GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayout.constraintCount = gridSize;
        gridLayout.cellSize = new Vector2(cellSize, cellSize);
        gridLayout.spacing = new Vector2(spacing, spacing);
    }

    public void OnMemoryCardClicked(MemoryCard card) 
    {
      

        if (card1 == null)
        {
            if (wrongMoveAmount == 0) { return; }

            wrongMoveAmount -= 1;

            card1 = card;
            card1.ShowCard(null);
            return;
        }
        else if (card2 == null) 
        {
            card2 = card;
            pendingMoves += 1;

            int card1TempID = card1.GetId();
            int card2TempID = card2.GetId();

            card2.ShowCard(() => 
            {
                CheckForCardMachig(card1TempID, card2TempID);
            });

            ClearHoldedCards();

        }

    }
    private int Score = 0;
    private void CheckForCardMachig(int card1Id, int card2Id) 
    {

        pendingMoves -= 1;

        if (memoryCardsDictionary[card1Id].GetCardSprite() == memoryCardsDictionary[card2Id].GetCardSprite())
        {
            memoryCardsDictionary[card1Id].isMatched = true;
            memoryCardsDictionary[card2Id].isMatched = true;
            wrongMoveAmount += 1;
            Score += 5;
            GameUiManager.instance.UpdateScoreText(Score);

            AudioManager.instance.PlaySound("CardMatching");

            if (AreAllCardsMatched())
            {
                GameUiManager.instance.ShowEndGamePanel(true);
                AudioManager.instance.PlaySound("Win");
                ProgressManager.ClearLevelData();
            }
        }
        else 
        {
            memoryCardsDictionary[card1Id].HideCard(null, 0.3f);
            memoryCardsDictionary[card2Id].HideCard(null, 0.3f);

            GameUiManager.instance.UpdateMovesText(wrongMoveAmount);
            AudioManager.instance.PlaySound("CardMatchingFail");
        }

        if (wrongMoveAmount == 0 && pendingMoves == 0)
        {
            GameUiManager.instance.ShowEndGamePanel(false);
            ProgressManager.ClearLevelData();
        }
    }

    private void ClearHoldedCards()
    {
        card1 = null;
        card2 = null;
    }

    private bool AreAllCardsMatched() 
    {
        foreach (var card in memoryCardsDictionary.Values) 
        {
            if (!card.isMatched)
                return false;
        }

        return true;
    }

    public void LoadLevel() 
    {
        LevelData savedLevelData = ProgressManager.LoadLevel();

        if (savedLevelData.cards.Count != totalCardsCouple * 2)
        {
            ProgressManager.ClearLevelData();
            return;
        }

        wrongMoveAmount = savedLevelData.remainingMovesCount;
        Score = savedLevelData.score;

        for (int i = 0; i < savedLevelData.cards.Count; i++) 
        {
            memoryCards[i].InitCard(savedLevelData.cards[i].cardId, GetSpriteByName(savedLevelData.cards[i].cardSprite), this);
            if (savedLevelData.cards[i].isVisible)
                memoryCards[i].ForceShow();
            else 
                memoryCards[i].ForceHide();
        }
    }

    public Sprite GetSpriteByName(string name) 
    {
        for (int i = 0; i < cardSprites.Count; i++) 
        {
            if (cardSprites[i].name == name)
            {
                return cardSprites[i];
            }
        }

        return null;
    }

    public void SaveProgress()
    {
        LevelData levelData = new LevelData();

        levelData.remainingMovesCount = wrongMoveAmount;
        levelData.score = Score;
        levelData.cards = new List<CardData>();

        for (int i = 0; i < memoryCards.Count; i++) 
        {
            levelData.cards.Add(new CardData
            {
                cardId = memoryCards[i].GetId(),
                isVisible = memoryCards[i].isMatched,
                cardSprite = memoryCards[i].GetCardSprite().name
            });
        }

        ProgressManager.SaveLevel(levelData);
    }
}
