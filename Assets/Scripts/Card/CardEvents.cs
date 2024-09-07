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
    public static List<string> COCaumentos = new List<string>();
    public static List<string> CRaumentos = new List<string>();    
    
    public GameObject Cardstats;
    public TextMeshProUGUI Powerstat;
    public Image Cardimage;

    


    private void Start() 
    {
        COCaumentos.Add("COCAumento (M)");
        COCaumentos.Add("COCAumento (R)");
        COCaumentos.Add("COCAumento (S)");
        CRaumentos.Add("CRAumento (M)");
        CRaumentos.Add("CRAumento (R)");
        CRaumentos.Add("CRAumento (S)");
    }
    public void Click()
    {
        GameObject COCHand = GameObject.Find("COCHand");
        GameObject CRHand = GameObject.Find("CRHand");
        CardDisplay keyword = Playercard.GetComponent<CardDisplay>();
        Effects activate = Playercard.GetComponent<Effects>();
                
        if (Playercard.transform.parent == COCHand.transform && TurnSystem.turn ==1 )
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
                    Field = GameObject.Find("COCMelee");
                    Playercard.transform.SetParent(Field.transform, false);
                }
                else if (row == "Ranged" )
                {
                    Field = GameObject.Find("COCRange");
                    Playercard.transform.SetParent(Field.transform, false);
                }
                else if (row == "Siege" )
                {
                    Field = GameObject.Find("COCSiege");
                    Playercard.transform.SetParent(Field.transform, false);
                }
            }    
            else if (cardDisplay.cardtype == "Clima" )
            {
                Field = GameObject.Find("ClimaZone");
                Playercard.transform.SetParent(Field.transform, false);
            }
            else if (cardDisplay.cardtype == "Aumento" )
            {
                List<string> aumentos = COCaumentos;
                int random;
                random = Random.Range(0, aumentos.Count);
                Field = GameObject.Find(aumentos[random]);
                Playercard.transform.SetParent(Field.transform, false);
                COCaumentos.RemoveAt(random);
            }
            else if (cardDisplay.cardtype == "Despeje")
            {

            }
            else if (cardDisplay.cardtype == "Señuelo")
            {

            }
            
            TurnSystem.turn = 0;

        }

        if (Playercard.transform.parent == CRHand.transform && TurnSystem.turn == 0)
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
                    Field = GameObject.Find("CRMelee");
                    Playercard.transform.SetParent(Field.transform, false);
                }
                else if (row == "Ranged" )
                {
                    Field = GameObject.Find("CRRange");
                    Playercard.transform.SetParent(Field.transform, false);
                }
                else if (row == "Siege" )
                {
                    Field = GameObject.Find("CRSiege");
                    Playercard.transform.SetParent(Field.transform, false);
                }
            }
            else if (cardDisplay.cardtype == "Clima" )
            {
                Field = GameObject.Find("ClimaZone");
                Playercard.transform.SetParent(Field.transform, false);
            }
            else if (cardDisplay.cardtype == "Aumento" )
            {
                List<string> aumentos = CRaumentos;
                int random ;
                random = Random.Range(0, aumentos.Count);
                Field = GameObject.Find(aumentos[random]);
                Playercard.transform.SetParent(Field.transform, false);
                CRaumentos.RemoveAt(random);
            }
            else if (cardDisplay.cardtype == "Despeje")
            {

            }
            else if (cardDisplay.cardtype == "Señuelo")
            {
                
            }
            TurnSystem.turn = 1;
        }
    }

    public void HoverEnter()
    {
        Cardstats = GameObject.Find("Stats");
        GameObject COCHand = GameObject.Find("COCHand");
        GameObject CRHand = GameObject.Find("CRHand");
        Cardimage = Cardstats.GetComponent<Image>();
        GameObject hideObject = Cardstats.transform.Find("Hide")?.gameObject;
        Powerstat = hideObject.transform.Find("Power")?.GetComponent<TextMeshProUGUI>();
        


        if (Playercard.transform.parent != CRHand.transform && TurnSystem.turn ==1 )
        {
            CardDisplay stats= Playercard.GetComponent<CardDisplay>();
            Cardimage.sprite = stats.spriteimage;
            Powerstat.text = stats.power.ToString();
        }
        if (Playercard.transform.parent != COCHand.transform && TurnSystem.turn == 0)
        {
            CardDisplay stats= Playercard.GetComponent<CardDisplay>();
            Cardimage.sprite = stats.spriteimage;
            Powerstat.text = stats.power.ToString();
        }
        
    }

    
}
