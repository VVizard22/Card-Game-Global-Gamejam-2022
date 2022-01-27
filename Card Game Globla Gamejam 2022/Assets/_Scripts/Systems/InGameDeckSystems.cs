using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameDeckSystem : MonoBehaviour
{
    public class deckClass<T>
    {
        private List<T> inGameDeck;
        private static System.Random rng = new System.Random();

        public deckClass(Dictionary<string, int> deckReference)
        {
            foreach (KeyValuePair<string, int> entry in deckReference)
            {
                for (int i = 1; i < entry.Value; i++)
                {
                    inGameDeck.Add(ResourceSystem.Instance.GetCard(entry.Key).Duplicate());
                }
            }
            Shuffle();
        }

        public void Shuffle()
        {
            int n = inGameDeck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = inGameDeck[k];
                inGameDeck[k] = inGameDeck[n];
                inGameDeck[n] = value;
            }
        }

        public T Draw()
        {
            int head = inGameDeck.Count - 1;
            T aux;

            aux = inGameDeck[head];
            inGameDeck.RemoveAt(head);

            return aux;
        }
    }
}
