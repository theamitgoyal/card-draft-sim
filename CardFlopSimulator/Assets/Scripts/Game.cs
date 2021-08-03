using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private int cardsPerFlop = 4;
    public int CardsPerFlop => cardsPerFlop;
    [SerializeField] private int noOfTurns = 3;
    public int NoOfTurns => noOfTurns;

    private float[,] cardDrawProbabilities;
    public float[,] CardDrawProbabilities => cardDrawProbabilities;
   
    private int[,] remainingCards;
    public int[,] RemainingCards => remainingCards;
   
    private string[,] flopOutput;
    public string[,] FlopOutput => flopOutput;
    
    private Deck deck;
    private void Awake()
    {
        
        deck = FindObjectOfType<Deck>();

        cardDrawProbabilities = new float[noOfTurns, deck.CardSets.Length];
        remainingCards = new int[noOfTurns, deck.CardSets.Length];
        flopOutput = new string[noOfTurns, cardsPerFlop];
    }
    
    public void Reset()
    {
        deck.GenerateShuffledDeck();
    }

    public void Run()
    {
        for (int i = 0; i < noOfTurns; i++)
        {
            if (deck.ShuffledCards.Count <= 0)
            {
                Debug.LogWarning("Attempt to Draw from Empty Deck. Please check simulation settings");
                return;
            }
            
            //Calculate Remaining Cards and Draw Probability values for each type
            int counter = 0;
            foreach (var set in deck.CardSets)
            {
                int numberofCards = deck.ShuffledCards.FindAll(c => c.Type == set.type).Count;
                remainingCards[i, counter] = numberofCards;
                float drawProbability = 0;
                if (deck.ShuffledCards != null)
                {
                    drawProbability = numberofCards * 100 / deck.ShuffledCards.Count;
                }
                cardDrawProbabilities[i, counter] = drawProbability;
                counter++;
            }

            //Store Flop Output for the turn
            int toDraw = deck.ShuffledCards.Count >= cardsPerFlop ? cardsPerFlop : deck.ShuffledCards.Count;

            for (int j = 0; j < toDraw; j++)
            {
                Card card = deck.DrawCard();
                flopOutput[i, j] = card.Type.ToString();
            }
        }
    }
}
