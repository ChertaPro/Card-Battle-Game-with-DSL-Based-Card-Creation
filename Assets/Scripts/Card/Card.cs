using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Card  
{
    public int id;
    public int owner;
    public string cardname;
    public string cardtype;
    public string faction;
    public int? power;
    public List<string> range;
    public string effect;
    public Sprite spriteimage;
    public bool climabool;
    public bool aumentobool;


    public Card()
    {

    }

    public Card(int Id,int owner, string Cardname,string Cardtype,string Faction, int? Power, List<string> Range,string Effect,Sprite Spriteimage,bool Climabool,bool Aumentobool)
    {
        this.id = Id;
        this.owner = owner;
        this.cardname = Cardname;
        this.cardtype = Cardtype;
        this.faction = Faction;
        this.power = Power;
        this.range = Range;
        this.effect = Effect;
        this.spriteimage = Spriteimage;
        this.climabool = Climabool;
        this.aumentobool = Aumentobool;
    }
    
}

