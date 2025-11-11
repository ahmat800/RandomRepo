using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MemoryCardsManager : MonoBehaviour
{


    public int maxTryCount = 5;
    private MemoryCard card1;
    private MemoryCard card2;

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
            card2.ShowCard(() => 
            {
                CheckForCardMachig(card1, card2);
            });
        }

    }

    public void CheckForCardMachig(MemoryCard card1, MemoryCard card2) 
    {
        if (card1.GetCardSprite() == card2.GetCardSprite())
        {
            // Game Is Done
        }
        else 
        {
            card1.HideCard(null);
            card2.HideCard(null);
        }
    }





}
