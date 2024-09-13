using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int Id;
    public GameObject Leader;
    public GameObject Hand;
    public List<Card> Deck;
    public GameObject Melee;
    public GameObject Range;
    public GameObject Siege;
    public GameObject Aumento_M;
    public GameObject Aumento_R;
    public GameObject Aumento_S;
    public GameObject Clima;
    public GameObject Graveyard;
    public GameObject TotalPower;
    public GameObject Rounds;

    public Player()
    {

    }

    public Player(int Id ,GameObject Leader, GameObject Hand,List<Card> Deck, GameObject Melee, GameObject Range, GameObject Siege, GameObject Aumento_M, GameObject Aumento_R, GameObject Aumento_S, GameObject Clima, GameObject Graveyard, GameObject TotalPower, GameObject Rounds) 
    {
        this.Id = Id;
        this.Leader = Leader;
        this.Hand = Hand;
        this.Deck = Deck;
        this.Melee = Melee;
        this.Range = Range;
        this.Siege = Siege;
        this.Aumento_M = Aumento_M;
        this.Aumento_R = Aumento_R;
        this.Aumento_S = Aumento_S;
        this.Clima = Clima;
        this.Graveyard = Graveyard;
        this.TotalPower = TotalPower;
        this.Rounds = Rounds;
    }

}


