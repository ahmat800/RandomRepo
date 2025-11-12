using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MemoryCard : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardBackgroundImage;
    [SerializeField] private Sprite flippedCardSprite;
    [SerializeField] private Sprite cardbackgroundSprite;
    [SerializeField] private Button button;


    [SerializeField] private float cardFlipDuration;
    private int Id;

    public void InitCard(int cardId, Sprite cardSprite, MemoryCardsManager cardsManager)
    {
        Id = cardId;
        cardImage.sprite = cardSprite;
        button.onClick.AddListener(() =>
        {
            cardsManager.OnMemoryCardClicked(this);
        });
    }

    public int GetId() 
    {
        return Id;
    }

    Coroutine cardFlipCoroutine;

    public void ShowCard(UnityAction onComplete, float delay = 0)
    {
        if (cardFlipCoroutine != null) 
            StopCoroutine(cardFlipCoroutine);

        cardFlipCoroutine = StartCoroutine(ScaleOnX(true, onComplete, delay));

    }
    public void HideCard(UnityAction onComplete, float delay = 0) 
    {
        if (cardFlipCoroutine != null)
            StopCoroutine(cardFlipCoroutine);

        cardFlipCoroutine = StartCoroutine(ScaleOnX(false, onComplete, delay));
    }

    private IEnumerator ScaleOnX(bool cardVisibilaty, UnityAction onComplete, float delay = 0)
    {
        float timer = 0;
        if (cardVisibilaty) 
        {
            button.interactable = false;
        } 

        yield return new WaitForSeconds(delay);


        while (timer / cardFlipDuration < 1) 
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(1, 0, (timer / cardFlipDuration));
            Debug.LogError($"Scale {scale}");
            cardBackgroundImage.rectTransform.localScale = new Vector3(scale, 1, 1);
            yield return null;
        }

        if (cardVisibilaty) 
        {
            cardImage.enabled = true;
            cardBackgroundImage.sprite = cardbackgroundSprite;
        }
        else
        {
            cardImage.enabled = false;
            cardBackgroundImage.sprite = flippedCardSprite;
        }

        timer = 0;
        while (timer / cardFlipDuration < 1)
        {
            timer += Time.deltaTime;
            float scale = Mathf.Lerp(0, 1, (timer / cardFlipDuration));
            cardBackgroundImage.rectTransform.localScale = new Vector3(scale, 1 ,1);
            yield return null;
        }

        button.interactable = !cardVisibilaty;
        onComplete?.Invoke();
    }

    public Sprite GetCardSprite() 
    {
        return cardImage.sprite;
    }
}
