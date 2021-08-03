using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDraft : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] int noOfPlayers = 4;

    [Header("No. of cards")]
    [SerializeField] int commonCards = 5;
    [SerializeField] int uncommonCards = 5;
    [SerializeField] int rareCards = 5;

    [Header("Card Weights")]
    [SerializeField] int commonCardWeight = 100;
    [SerializeField] int uncommonCardWeight = 50;
    [SerializeField] int rareCardWeight = 25;

    [Header("Auto Generate")]
    [SerializeField] bool generateMultipleRuns = false;
    [SerializeField] int numberOfRuns = 8;

    [Header("References")]
    [SerializeField] Text commonCardsText;
    [SerializeField] Text uncommonCardsText;
    [SerializeField] Text rareCardsText;
    [SerializeField] Text draftIndexText;
    [SerializeField] GameObject commonCardPrefab;
    [SerializeField] GameObject uncommonCardPrefab;
    [SerializeField] GameObject rareCardPrefab;
    [SerializeField] Transform displayParentObject;

    int currentDeckValue = 0;
    int commonCardsInDeck = 0;
    int uncommonCardsInDeck = 0;
    int rareCardsInDeck = 0;

    int draftIndex = 0;
    int commonCardsInDraft = 0;
    int uncommonCardsInDraft = 0;
    int rareCardsInDraft = 0;

    public int DraftIndex { get => draftIndex; }
    public int CommonCardsInDraft { get => commonCardsInDraft; }
    public int UncommonCardsInDraft { get => uncommonCardsInDraft; }
    public int RareCardsInDraft { get => rareCardsInDraft; }
    public bool GenerateMultipleRuns { get => generateMultipleRuns; }


    public Action sendData;
   
    private void Start()
    {
        commonCardsInDeck = commonCards;
        uncommonCardsInDeck = uncommonCards;
        rareCardsInDeck = rareCards;
        commonCardsText.text = string.Format("Common Cards: {0}", commonCardsInDeck);
        uncommonCardsText.text = string.Format("Uncommon Cards: {0}", uncommonCardsInDeck);
        rareCardsText.text = string.Format("Rare Cards: {0}", rareCardsInDeck);
        currentDeckValue = SetDeckValue();
        Debug.Log(string.Format("Start Deck Value: {0}", currentDeckValue));

        if (generateMultipleRuns) PerformMultipleRuns();
    }

    private void PerformMultipleRuns()
    {
        for (int i = 0; i < numberOfRuns; i++)
        {
            DraftCards(5);
        }
    }


    public void DraftCards(int number)
    {
        ClearCards();

        draftIndex++;
        draftIndexText.text = string.Format("Draft Index: {0}", draftIndex);

        int cardsInDeck = commonCardsInDeck + uncommonCardsInDeck + rareCardsInDeck;
        if (cardsInDeck < number)
        {
            Debug.Log("Insufficient Cards");
            return;
        }

        for (int i = 0; i < number; i++)
        {
            DrawCard(i);
        }

        //Data export
        sendData();
        Debug.Log(string.Format("Draft Index: {0}, Common: {1}, Uncommon: {2}, Rare: {3}", draftIndex, commonCardsInDraft, uncommonCardsInDraft, rareCardsInDraft));
    }

    private int SetDeckValue()
    {
        int deckValue = (commonCardsInDeck * commonCardWeight) + (uncommonCardsInDeck * uncommonCardWeight) + (rareCardsInDeck * rareCardWeight);
        return deckValue;
    }

    private void DrawCard(int index)
    {
        int drawValue = UnityEngine.Random.Range(1, currentDeckValue + 1);

        if (drawValue < commonCardsInDeck * commonCardWeight)
        {
            commonCardsInDeck--;
            commonCardsInDraft++;
            commonCardsText.text = string.Format("Common Cards: {0}", commonCardsInDeck);
            DisplayCard(commonCardPrefab, index);
            currentDeckValue = SetDeckValue();
        }
        else if (drawValue < (commonCardsInDeck * commonCardWeight) + (uncommonCardsInDeck * uncommonCardWeight))
        {
            uncommonCardsInDeck--;
            uncommonCardsInDraft++;
            uncommonCardsText.text = string.Format("Uncommon Cards: {0}", uncommonCardsInDeck);
            DisplayCard(uncommonCardPrefab, index);
            currentDeckValue = SetDeckValue();
        }
        else
        {
            rareCardsInDeck--;
            rareCardsInDraft++;
            rareCardsText.text = string.Format("Rare Cards: {0}", rareCardsInDeck);
            DisplayCard(rareCardPrefab, index);
            currentDeckValue = SetDeckValue();
        }
    }

    private void DisplayCard(GameObject cardPrefab, int draftIndex)
    {
        Vector3 cardPos = new Vector3(1.5f * draftIndex, 0f, 0f);
        GameObject card = Instantiate(cardPrefab, cardPos, Quaternion.identity);
        card.transform.parent = displayParentObject;
    }

    private void ClearCards()
    {
        if (displayParentObject.childCount < 1) return;

        if (generateMultipleRuns) SimulatePlayerPickup();

        foreach(Transform child in displayParentObject)
        {
            if (child.CompareTag("Common"))
            {
                commonCardsInDeck++;
                currentDeckValue = SetDeckValue();
                commonCardsText.text = string.Format("Common Cards: {0}", commonCardsInDeck);
            }
            else if (child.CompareTag("Uncommon"))
            {
                uncommonCardsInDeck++;
                currentDeckValue = SetDeckValue();
                uncommonCardsText.text = string.Format("Uncommon Cards: {0}", uncommonCardsInDeck);
            }
            else if (child.CompareTag("Rare"))
            {
                rareCardsInDeck++;
                currentDeckValue = SetDeckValue();
                rareCardsText.text = string.Format("Rare Cards: {0}", rareCardsInDeck);
            }
            
            Destroy(child.gameObject);
        }

        commonCardsInDraft = 0;
        uncommonCardsInDraft = 0;
        rareCardsInDraft = 0;

        Debug.Log(string.Format("Current Deck: Common: {0}, Uncommon: {1}, Rare: {2}", commonCardsInDeck, uncommonCardsInDeck, rareCardsInDeck));
    }

    private void SimulatePlayerPickup()
    {

        for (int i = 0; i < noOfPlayers; i++)
        {
            if (rareCardsInDraft > 0)
            {
                rareCardsInDraft--;
                Destroy(GameObject.FindWithTag("Rare"));
                Debug.Log("Rare card picked");
            }
            else if (uncommonCardsInDraft > 0)
            {
                uncommonCardsInDraft--;
                Destroy(GameObject.FindWithTag("Uncommon"));
                Debug.Log("Uncommon card picked");
            }
            else if (commonCardsInDraft > 0)
            {
                commonCardsInDraft--;
                Destroy(GameObject.FindWithTag("Common"));
                Debug.Log("Common card picked");
            }
        }
    }
}
