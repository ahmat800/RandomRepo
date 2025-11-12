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

    public void InitCard(Sprite cardSprite, MemoryCardsManager cardsManager)
    {
        cardImage.sprite = cardSprite;
        button.onClick.AddListener(() =>
        {
            cardsManager.OnMemoryCardClicked(this);
        });
    }

    Coroutine cardFlipCoroutine;

    public void ShowCard(UnityAction onComplete)
    {
        if (cardFlipCoroutine != null) 
            StopCoroutine(cardFlipCoroutine);

        cardFlipCoroutine = StartCoroutine(ScaleOnX(true, onComplete));

    }
    public void HideCard(UnityAction onComplete) 
    {
        if (cardFlipCoroutine != null)
            StopCoroutine(cardFlipCoroutine);

        cardFlipCoroutine = StartCoroutine(ScaleOnX(false, onComplete));
    }

    private IEnumerator ScaleOnX(bool cardVisibilaty, UnityAction onComplete)
    {
        float timer = 0;
        if (cardVisibilaty) 
        {
            button.interactable = false;
        } 

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

        if (!cardVisibilaty)
        {
            button.interactable = true;
        }

        onComplete?.Invoke();
    }

    public Sprite GetCardSprite() 
    {
        return cardImage.sprite;
    }
}
