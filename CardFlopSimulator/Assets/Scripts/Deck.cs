using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Deck : MonoBehaviour
{
   [System.Serializable]
   public struct CardSet
   {
       public CardType type;
       public int numberOfCards;
   }

   [SerializeField] private CardSet[] cardSets;
   public CardSet[] CardSets => cardSets; 
   
   private List<Card> cards = new List<Card>();
   
   private List<Card> shuffledCards = new List<Card>();
   public List<Card> ShuffledCards => shuffledCards;

   private static Random rng = new Random(); // Used to shuffle deck
   
   public void GenerateShuffledDeck()
   {
       if (cardSets.Length == 0) return;
       
       cards.Clear();
       shuffledCards.Clear();
       
       cards = GenerateDeck();
       shuffledCards = ShuffleDeck(cards);
   }

   private List<Card> GenerateDeck()
   {
       List<Card> cards = new List<Card>();
       
       for (int i = 0; i < cardSets.Length; i++)
       {
           for (int j = 0; j < cardSets[i].numberOfCards; j++)
           {
               cards.Add(new Card(cardSets[i].type));
           }
       }
       
       return cards;
   }
   private List<Card> ShuffleDeck(List<Card> cards)
   {
       return cards.OrderBy(c => rng.Next()).ToList();
   }

   public Card DrawCard()
   {
       Card card = shuffledCards[0];
       shuffledCards.RemoveAt(0);
       return card;
   }
}
