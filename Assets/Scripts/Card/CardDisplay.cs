using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardDisplay : MonoBehaviour
{

    public Card displaycard;
    public int displayid;
    public int id;
    public int owner;
    public string cardname;
    public string cardtype;
    public string faction;
    public List<string> attack_type;
    public int? power;
    public string effect;
    public Sprite spriteimage;

    public Image show;

//**-------------------CardBack----------------------------------------
    public bool coccardback;
    public bool crcardback;
    public static bool cocstaticcardback;
    public static bool crstaticcardback;
//**-------------------Hand--------------------------------------------
    public GameObject COCHand;
    public GameObject CRHand;
//**-------------------Effects----------------------------------------- 
    public bool climabool;
    public bool aumentobool;
    public List<int?> backuppower; 
    public bool keepingbool;

    // Start is called before the first frame update
    void Start()
    {
        DisplayHand();
        DisplayLeader();
        keepingbool = false;
        backuppower = new List<int?>();
        if(CardDatabase.COCDeck.Count > 0 && CardDatabase.CRDeck.Count > 0)
        {
            foreach (Card card in CardDatabase.COCDeck)
            {
                backuppower.Add(card.power);
            }
            foreach (Card card in CardDatabase.CRDeck)
            {
                backuppower.Add(card.power);
            }

        }
        

    }

    // Update is called once per frame
    void Update()
    {
        Display();
        cocstaticcardback = coccardback;
        crstaticcardback = crcardback;
        DisplayCardBack();
        Gaveyard();

    }
    void Display()
    {
        id = displaycard.id;
        owner = displaycard.owner;
        cardname = displaycard.cardname;
        cardtype = displaycard.cardtype;
        faction = displaycard.faction;
        attack_type = displaycard.range;
        power = displaycard.power;
        effect = displaycard.effect;
        spriteimage = displaycard.spriteimage;
        show.sprite = spriteimage;
        climabool = displaycard.climabool;
        aumentobool = displaycard.aumentobool;
        
    }

    void DisplayLeader()
    {
        GameObject leadercoc = CardDatabase.player1.Leader.transform.GetChild(0).gameObject;
        CardDisplay cocdisplay = leadercoc.GetComponent<CardDisplay>();
        cocdisplay.displaycard = CardDatabase.Leaders[0];
        GameObject leadercr = CardDatabase.player2.Leader.transform.GetChild(0).gameObject;
        CardDisplay crdisplay = leadercr.GetComponent<CardDisplay>();
        crdisplay.displaycard = CardDatabase.Leaders[1];
    }
    void DisplayHand()
    {
        if (this.transform.parent == CardDatabase.player1.Hand.transform)
        {
            GameObject clone = gameObject;
            displaycard = CardDatabase.COCDeck[0];
            CardDatabase.COCDeck.RemoveAt(0);
            coccardback = false;
            crcardback = false;            
        }
        if (this.transform.parent == CardDatabase.player2.Hand.transform)
        {
            GameObject clone = gameObject;
            displaycard = CardDatabase.CRDeck[0];
            CardDatabase.CRDeck.RemoveAt(0);
            coccardback = false;
            crcardback = false;
        }
    }
    void DisplayCardBack()
    {
        if (TurnSystem.turn == 1 && gameObject.transform.parent == CardDatabase.player2.Hand.transform)
        {
            crcardback = true;
        }
        if (TurnSystem.turn == 1 && gameObject.transform.parent == CardDatabase.player1.Hand.transform)
        {
            coccardback = false;
            crcardback = false;
        }
        if (TurnSystem.turn == 0 && gameObject.transform.parent == CardDatabase.player1.Hand.transform)
        {
            coccardback = true;
        }
        if (TurnSystem.turn == 0 && gameObject.transform.parent == CardDatabase.player2.Hand.transform)
        {
            coccardback = false;
            crcardback = false;
        }
        if (TurnSystem.turn == 2 && gameObject.transform.parent == CardDatabase.player1.Hand.transform)
        {
            coccardback = true;
        }
        if (TurnSystem.turn == 2 && gameObject.transform.parent == CardDatabase.player2.Hand.transform)
        {
            crcardback = true;
        }
    }
    void Gaveyard()
    {
        if (gameObject.transform.parent == CardDatabase.player1.Graveyard.transform && effect == "melee")
        {
            foreach(Transform card in GameElements.Board())
            {
                CardDisplay melee = card.GetComponent<CardDisplay>();
                melee.climabool = true;
                if(melee.faction == "COC")
                {
                    melee.power = CardDatabase.COCbackup[melee.id-1].power;
                }
                else
                {
                    melee.power = CardDatabase.CRbackup[melee.id-1].power;
                }
                Destroy(gameObject,1.3f);
            }
        }
        if (gameObject.transform.parent == CardDatabase.player1.Graveyard.transform && effect == "range")
        {
            foreach(Transform card in GameElements.Board())
            {
                CardDisplay range = card.GetComponent<CardDisplay>();
                range.climabool = true;
                if(range.faction == "COC")
                {
                    range.power = CardDatabase.COCbackup[range.id-1].power;
                }
                else
                {
                    range.power = CardDatabase.CRbackup[range.id-1].power;
                }
                Destroy(gameObject,1.3f);
            }
        }
        if (gameObject.transform.parent == CardDatabase.player2.Graveyard.transform && effect == "melee")
        {
            foreach(Transform card in GameElements.Board())
            {
                CardDisplay melee = card.GetComponent<CardDisplay>();
                melee.climabool = true;
                if(melee.faction == "COC")
                {
                    melee.power = CardDatabase.COCbackup[melee.id-1].power;
                }
                else
                {
                    melee.power = CardDatabase.CRbackup[melee.id-1].power;
                }
                Destroy(gameObject,1.3f);
            }
        }
        if (gameObject.transform.parent == CardDatabase.player2.Graveyard.transform && effect == "range")
        {
            foreach(Card card in CardDatabase.CRDeck)
            {
                if (card.range !=null)
                {
                    card.climabool = true;
                    card.power = backuppower[card.id];
                    Destroy(gameObject,1.3f);
                }
            }
        }
        if (gameObject.transform.parent == CardDatabase.player1.Graveyard.transform && effect == "bonus")
        {
            foreach(Transform card in GameElements.Board())
            {
                CardDisplay aumento = card.GetComponent<CardDisplay>();
                aumento.aumentobool = true;
                if(aumento.faction == "COC")
                {
                    aumento.power = CardDatabase.COCbackup[aumento.id-1].power;
                }
                else
                {
                    aumento.power = CardDatabase.CRbackup[aumento.id-1].power;
                }
                Destroy(gameObject,1.3f);
            }    
        }
        if (gameObject.transform.parent == CardDatabase.player2.Graveyard.transform && effect == "bonus")
        {
            foreach(Transform card in GameElements.Board())
            {
                CardDisplay aumento = card.GetComponent<CardDisplay>();
                aumento.aumentobool = true;
                if(aumento.faction == "COC")
                {
                    aumento.power = CardDatabase.COCbackup[aumento.id-1].power;
                }
                else
                {
                    aumento.power = CardDatabase.CRbackup[aumento.id-1].power;
                }
                Destroy(gameObject,1.3f);
            }
        }
        if (gameObject.transform.parent == CardDatabase.player1.Graveyard.transform || gameObject.transform.parent == CardDatabase.player2.Graveyard.transform)
        {
            Destroy(gameObject,1.3f);
        }
    }
}
