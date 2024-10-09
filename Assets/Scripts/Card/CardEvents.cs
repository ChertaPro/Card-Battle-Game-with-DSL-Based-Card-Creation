using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class CardEvents : MonoBehaviour
{
    public GameObject Playercard;
    [HideInInspector]
    public GameObject Field;
    public static List<GameObject> COCaumentos = new List<GameObject>();
    public static List<GameObject> CRaumentos = new List<GameObject>();    
    
    public GameObject Cardstats;
    public TextMeshProUGUI Powerstat;
    public TextMeshProUGUI Namestat;
    public TextMeshProUGUI TypeStat;
    public Image Cardimage;

    


    private void Start() 
    {
        COCaumentos.Add(CardDatabase.player1.Aumento_M);
        COCaumentos.Add(CardDatabase.player1.Aumento_R);
        COCaumentos.Add(CardDatabase.player1.Aumento_S);
        CRaumentos.Add(CardDatabase.player2.Aumento_M);
        CRaumentos.Add(CardDatabase.player2.Aumento_R);
        CRaumentos.Add(CardDatabase.player2.Aumento_S);
    }
    public void Click()
    {
        GameObject COCHand = GameObject.Find("COCHand");
        GameObject CRHand = GameObject.Find("CRHand");
        CardDisplay keyword = Playercard.GetComponent<CardDisplay>();
        Effects activate = Playercard.GetComponent<Effects>();
                
        if (Playercard.transform.parent == CardDatabase.player1.Hand.transform && TurnSystem.turn ==1 )
        {
            activate.Effect(keyword.effect,Playercard);
            CardDisplay cardDisplay = Playercard.GetComponent<CardDisplay>();
            if(cardDisplay.attack_type != null)
            {
                int ran;
                ran = Random.Range(0,cardDisplay.attack_type.Count);
                string row = cardDisplay.attack_type[ran];

                if (row == "Melee" )
                {
                    Field = CardDatabase.player1.Melee;
                    Playercard.transform.SetParent(Field.transform, false);
                }
                else if (row == "Ranged" )
                {
                    Field = CardDatabase.player1.Range;
                    Playercard.transform.SetParent(Field.transform, false);
                }
                else if (row == "Siege" )
                {
                    Field = CardDatabase.player1.Siege;
                    Playercard.transform.SetParent(Field.transform, false);
                }
            }    
            else if (cardDisplay.cardtype == "Clima" )
            {
                Field = CardDatabase.player1.Clima;
                Playercard.transform.SetParent(Field.transform, false);
            }
            else if (cardDisplay.cardtype == "Aumento" )
            {
                List<GameObject> aumentos = COCaumentos;
                int random;
                random = Random.Range(0, aumentos.Count);
                Field = aumentos[random];
                Playercard.transform.SetParent(Field.transform, false);
                COCaumentos.RemoveAt(random);
            }
            TurnSystem.turn = 0;

        }

        if (Playercard.transform.parent == CardDatabase.player2.Hand.transform && TurnSystem.turn == 0)
        {
            activate.Effect(keyword.effect,Playercard);
            CardDisplay cardDisplay = Playercard.GetComponent<CardDisplay>();
            if(cardDisplay.attack_type != null)
            {
                int ran;
                ran = Random.Range(0,cardDisplay.attack_type.Count);
                string row = cardDisplay.attack_type[ran];

                if (row == "Melee" )
                {
                    Field = CardDatabase.player2.Melee;
                    Playercard.transform.SetParent(Field.transform, false);
                }
                else if (row == "Ranged" )
                {
                    Field = CardDatabase.player2.Range;
                    Playercard.transform.SetParent(Field.transform, false);
                }
                else if (row == "Siege" )
                {
                    Field = CardDatabase.player2.Siege;
                    Playercard.transform.SetParent(Field.transform, false);
                }
            }
            else if (cardDisplay.cardtype == "Clima" )
            {
                Field = CardDatabase.player2.Clima;;
                Playercard.transform.SetParent(Field.transform, false);
            }
            else if (cardDisplay.cardtype == "Aumento" )
            {
                List<GameObject> aumentos = CRaumentos;
                int random ;
                random = Random.Range(0, aumentos.Count);
                Field = aumentos[random];
                Playercard.transform.SetParent(Field.transform, false);
                CRaumentos.RemoveAt(random);
            }
            TurnSystem.turn = 1;
        }
    }

    public void HoverEnter()
    {
        Cardstats = GameObject.Find("Stats");
        GameObject COCHand = CardDatabase.player1.Hand;;
        GameObject CRHand = CardDatabase.player2.Hand;;
        Cardimage = Cardstats.GetComponent<Image>();
        GameObject hideObject = Cardstats.transform.Find("Hide")?.gameObject;
        Powerstat = hideObject.transform.Find("Power")?.GetComponent<TextMeshProUGUI>();
        GameObject hide1 = Cardstats.transform.Find("Hide1")?.gameObject;
        Namestat = hide1.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
        GameObject hide2 = Cardstats.transform.Find("Hide 2")?.gameObject;
        TypeStat = hide2.transform.Find("Type")?.GetComponent<TextMeshProUGUI>();


        if (Playercard.transform.parent != CRHand.transform && TurnSystem.turn ==1 )
        {
            CardDisplay stats = Playercard.GetComponent<CardDisplay>();
            Cardimage.sprite = stats.spriteimage;
            Powerstat.text = stats.power.ToString();
            Namestat.text = stats.cardname.ToString();
            TypeStat.text = stats.cardtype.ToString();
        }
        if (Playercard.transform.parent != COCHand.transform && TurnSystem.turn == 0)
        {
            CardDisplay stats= Playercard.GetComponent<CardDisplay>();
            Cardimage.sprite = stats.spriteimage;
            Powerstat.text = stats.power.ToString();
            Namestat.text = stats.cardname.ToString();
            TypeStat.text = stats.cardtype.ToString();
        }
        
    }

    
}
