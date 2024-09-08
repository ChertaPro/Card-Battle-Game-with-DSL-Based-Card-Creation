using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Card  
{
    public int id;
    public string cardname;
    public string cardtype;
    public List<string> attack_type;
    public string faction;
    public int? power;
    public string effect;
    public Sprite spriteimage;
    public bool climabool;
    public bool aumentobool;


    public Card()
    {

    }

    public Card(int Id, string Cardname,string Cardtype, List<string> Attack_type,string Faction, int? Power,string Effect,Sprite Spriteimage,bool Climabool,bool Aumentobool)
    {
        this.id = Id;
        this.cardname = Cardname;
        this.cardtype = Cardtype;
        this.attack_type = Attack_type;
        this.faction = Faction;
        this.power = Power;
        this.effect = Effect;
        this.spriteimage = Spriteimage;
        this.climabool = Climabool;
        this.aumentobool = Aumentobool;
    }
    
}