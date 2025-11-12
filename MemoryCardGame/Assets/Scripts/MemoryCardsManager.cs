using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MemoryCardsManager : MonoBehaviour
{


    public int totalCardsCouple = 8;
    private Dictionary<int, MemoryCard> memoryCardsDictionary;
    public float cardsViewDuration = 4;
    public MemoryCard memoryCardPrefab;
    public GridLayoutGroup gridLayout;
    public List<Sprite> cardSprites;

    public int maxTryCount = 5;
    private MemoryCard card1;
    private MemoryCard card2;



    private void Start()
    {
        InitCards();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < memoryCardsDictionary.Count; i++)
            {
                memoryCardsDictionary[i].ShowCard(null);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space)) 
        {
            for (int i = 0; i < memoryCardsDictionary.Count; i++)
            {
                memoryCardsDictionary[i].HideCard(null);
            }
        }

    }

    public void InitCards()
    {
        InitCardsSize();

        List<MemoryCard> memoryCards = new List<MemoryCard>();
        memoryCardsDictionary = new Dictionary<int, MemoryCard> ();
        for (int i = 0; i < totalCardsCouple * 2; i += 2)
        {
            MemoryCard newCard1 = Instantiate(memoryCardPrefab);
            MemoryCard newCard2 = Instantiate(memoryCardPrefab);

            int radomCardSpriteIndex = UnityEngine.Random.Range(0, cardSprites.Count);
            newCard1.InitCard(i,cardSprites[radomCardSpriteIndex], this);
            newCard2.InitCard(i + 1, cardSprites[radomCardSpriteIndex], this);



            memoryCards.Add(newCard1);
            memoryCards.Add(newCard2);

        }

        ShuffleCards(memoryCards);

        foreach (MemoryCard card in memoryCards) 
        {
            memoryCardsDictionary.Add(card.GetId(), card);
        }

        for (int i = 0; i < memoryCardsDictionary.Count; i++)
        {
            memoryCardsDictionary[i].transform.SetParent(gridLayout.transform);
            memoryCardsDictionary[i].transform.localScale = Vector3.one;
        }
    }


    public void ShuffleCards(List<MemoryCard> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            int rand = Random.Range(i, cards.Count);
            (cards[i], cards[rand]) = (cards[rand], cards[i]);
        }
    }

    public void InitCardsSize()
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
            card1 = card;
            card1.ShowCard(null);
            return;
        }
        else if (card2 == null) 
        {
            card2 = card;
            maxTryCount -= 1;

            int card1TempID = card1.GetId();
            int card2TempID = card2.GetId();

            card2.ShowCard(() => 
            {
                CheckForCardMachig(card1TempID, card2TempID);
            });

            ClearHoldedCards();

        }

    }

    public void CheckForCardMachig(int card1Id, int card2Id) 
    {
        

        if (memoryCardsDictionary[card1Id].GetCardSprite() == memoryCardsDictionary[card2Id].GetCardSprite())
        {
            Debug.LogError("Matching Cards");
        }
        else 
        {
            memoryCardsDictionary[card1Id].HideCard(null, 0.3f);
            memoryCardsDictionary[card2Id].HideCard(null, 0.3f);
        }
    }

    public void ClearHoldedCards()
    {
        card1 = null;
        card2 = null;
    }





}
