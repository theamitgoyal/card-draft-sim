using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Simulation : MonoBehaviour
{
   [SerializeField] private int noOfGames = 5;
   [Header("References")]
   [SerializeField] private Text warningText;
   [SerializeField] private Button button;

   private Game game;
   private Deck deck;
   private string filename;

   private void Start()
   {
      game = FindObjectOfType<Game>();
      deck = FindObjectOfType<Deck>();
      warningText.enabled = false;
      filename = Application.dataPath + "/data.csv";

   }

   public void ExecuteAndWrite()
   {
      TextWriter writer = new StreamWriter(filename, false);
      string headings = GenerateHeadings();
      writer.WriteLine(headings);
      writer.Close();

      writer = new StreamWriter(filename, true);

      for (int i = 0; i < noOfGames; i++)
      {
         writer.WriteLine("GAME " + (i+1));
         game.Reset();
         game.Run();

         int toCount = (2 * deck.CardSets.Length) + game.CardsPerFlop + 1; // +1 for the Turn No column

         for (int j = 0; j < game.NoOfTurns; j++)
         {
            string data = "";
            for (int k = 0; k < toCount; k++)
            {

               if (k == 0)
               {
                  data = data + (j + 1) + ",";
               }
               else if (k > 0 && k <= deck.CardSets.Length)
               {
                  data = data + game.RemainingCards[j, k - 1] + ",";
               }
               else if (k > deck.CardSets.Length && k <= 2 * deck.CardSets.Length)
               {
                  float f = game.CardDrawProbabilities[j, k - deck.CardSets.Length - 1];
                  double value = Math.Round(f, 2);
                  data = data + value + ",";
               }
               else if (k > 2 * deck.CardSets.Length && k < toCount - 1)
               {
                  data = data + game.FlopOutput[j, k - (2 * deck.CardSets.Length) - 1] + ",";
               }
               else
               {
                  data = data + game.FlopOutput[j, k - (2 * deck.CardSets.Length) - 1];
               }
            }
            writer.WriteLine(data);
         }
      }
      writer.Close();
      warningText.enabled = true;
      button.gameObject.SetActive(false);
   }

   private string GenerateHeadings()
   {
      string s = "Turn No,";
      for (int i = 0; i < deck.CardSets.Length; i++)
      {
         s = s + deck.CardSets[i].type + " R,";
      }
      for (int j = 0; j < deck.CardSets.Length; j++)
      {
         s = s + deck.CardSets[j].type + " P,";
      }
      for (int k = 0; k < game.CardsPerFlop; k++)
      {
         if (k == game.CardsPerFlop - 1)
            s = s + "Flop " + (k + 1);
         else
            s = s + "Flop " + (k + 1) + ",";
      }

      return s;
   }
}
