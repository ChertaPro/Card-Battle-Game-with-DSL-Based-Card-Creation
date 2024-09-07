using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour
{

    public Card displaycard;
    public int displayid;
    public int id;
    public string cardname;
    public string cardtype;
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
        keepingbool = false;
        backuppower = new List<int?>();
        if(CardDatabase.cards.Count > 0)
        {
            foreach (Card card in CardDatabase.cards)
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
        COCHand = GameObject.Find("COCHand");
        CRHand = GameObject.Find("CRHand");
        displaycard = CardDatabase.cards[displayid];
        id = displaycard.id;
        cardname = displaycard.cardname;
        cardtype = displaycard.cardtype;
        attack_type = displaycard.attack_type;
        power = displaycard.power;
        effect = displaycard.effect;
        spriteimage = displaycard.spriteimage;
        show.sprite = spriteimage;
        climabool = displaycard.climabool;
        aumentobool = displaycard.aumentobool;
        
    }

    void DisplayHand()
    {
        COCHand = GameObject.Find("COCHand");
        CRHand = GameObject.Find("CRHand");
        if (this.transform.parent == COCHand.transform)
        {
            GameObject clone = gameObject;
            displayid = COCDeck.Staticdeck1[0].id;
            COCDeck.Staticdeck1.RemoveAt(0);
            coccardback = false;
            crcardback = false;            
        }
        if (this.transform.parent == CRHand.transform)
        {
            GameObject clone = gameObject;
            displayid = CRDeck.Staticdeck2[0].id;
            CRDeck.Staticdeck2.RemoveAt(0);
            coccardback = false;
            crcardback = false;
        }
    }
    void DisplayCardBack()
    {
        if (TurnSystem.turn == 1 && gameObject.transform.parent == CRHand.transform)
        {
            crcardback = true;
        }
        if (TurnSystem.turn == 1 && gameObject.transform.parent == COCHand.transform)
        {
            coccardback = false;
            crcardback = false;
        }
        if (TurnSystem.turn == 0 && gameObject.transform.parent == COCHand.transform)
        {
            coccardback = true;
        }
        if (TurnSystem.turn == 0 && gameObject.transform.parent == CRHand.transform)
        {
            coccardback = false;
            crcardback = false;
        }
        if (TurnSystem.turn == 2 && gameObject.transform.parent == COCHand.transform)
        {
            coccardback = true;
        }
        if (TurnSystem.turn == 2 && gameObject.transform.parent == CRHand.transform)
        {
            crcardback = true;
        }
    }
    void Gaveyard()
    {
        
        GameObject COCGraveyard = GameObject.Find("COCGraveyard");
        GameObject CRGraveyard = GameObject.Find("CRGraveyard");
        if (gameObject.transform.parent == COCGraveyard.transform || gameObject.transform.parent == CRGraveyard.transform)
        {
            CardDatabase.cards[displayid].power = backuppower[displayid];
            CardDatabase.cards[displayid].aumentobool = true;
            CardDatabase.cards[displayid].climabool = true;
            Destroy(gameObject,1.3f);
        }
        if (gameObject.transform.parent == COCGraveyard.transform && effect == "melee")
        {
            foreach(Card card in CardDatabase.cards)
            {
                if (card.attack_type !=null)
                {
                    card.climabool = true;
                    card.power = backuppower[card.id];
                    Destroy(gameObject,1.3f);
                }
            }
        }
        if (gameObject.transform.parent == COCGraveyard.transform && effect == "range")
        {
            foreach(Card card in CardDatabase.cards)
            {
                if (card.attack_type !=null)
                {
                    card.climabool = true;
                    card.power = backuppower[card.id];
                    Destroy(gameObject,1.3f);
                }
            }
        }
        if (gameObject.transform.parent == CRGraveyard.transform && effect == "melee")
        {
            foreach(Card card in CardDatabase.cards)
            {
                if (card.attack_type !=null)
                {
                    card.climabool = true;
                    card.power = backuppower[card.id];
                    Destroy(gameObject,1.3f);
                }
            }
        }
        if (gameObject.transform.parent == CRGraveyard.transform && effect == "range")
        {
            foreach(Card card in CardDatabase.cards)
            {
                if (card.attack_type !=null)
                {
                    card.climabool = true;
                    card.power = backuppower[card.id];
                    Destroy(gameObject,1.3f);
                }
            }
        }
        if (gameObject.transform.parent == COCGraveyard && effect == "bonus")
        {
            foreach(Card card in CardDatabase.cards)
            {
                card.aumentobool = true;
                card.power = backuppower[card.id];
                Destroy(gameObject,1.3f);
            }
        }
    }
}
