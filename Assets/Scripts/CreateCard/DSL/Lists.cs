using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lists 
    {
        public List<Card> Cards { get; set; }

        public Lists()
        {
            Cards = new List<Card>();
        }

        public Lists(List<Card> cards)
        {
            Cards = cards;
        }

        
    }