using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject card;
    public GameObject COCHand;
    public GameObject CRHand;
    public GameObject ClimaZone;
    public GameObject Field;
    public List<GameObject> aumentos = new List<GameObject>();
    public TextMeshProUGUI COCPowerCounter;
    public TextMeshProUGUI CRPowerCounter;
    public int? COCpower;
    public int? CRpower;
    public static int? staticCOCpower;
    public static int? staticCRpower;


    // Start is called before the first frame update
    void Start()
    {
        Shuffle(CardDatabase.COCDeck);
        Shuffle(CardDatabase.CRDeck);
        StartCoroutine("DrawCardsWithDelay");
        ClimaZone = GameObject.Find("ClimaZone");
        aumentos.Add(CardDatabase.player1.Aumento_M);
        aumentos.Add(CardDatabase.player1.Aumento_R);
        aumentos.Add(CardDatabase.player1.Aumento_S);
        aumentos.Add(CardDatabase.player2.Aumento_M);
        aumentos.Add(CardDatabase.player2.Aumento_R);
        aumentos.Add(CardDatabase.player2.Aumento_S);
    }

    // Update is called once per frame
    void Update()
    {
        PowerCounter();
        Clima();
        Aumento();
        CardCounter();
        staticCOCpower = COCpower;
        staticCRpower = CRpower;
    }

    public static void Shuffle(List<Card> cards)
    {
        int n = cards.Count;

        // Algoritmo de Fisher-Yates para mezclar la lista
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            // Intercambiar cards[i] con cards[j]
            Card temp = cards[i];
            cards[i] = cards[j];
            cards[j] = temp;
        }
    }
    public void COCDraw()
    {   
        GameObject COCcard = Instantiate(card,new Vector3(0, 0, 0), Quaternion.identity);
        COCcard.transform.SetParent(CardDatabase.player1.Hand.transform,false);
    }
    public void CRDraw()
    {
        GameObject CRcard = Instantiate(card,new Vector3(0, 0, 0), Quaternion.identity);
        CRcard.transform.SetParent(CardDatabase.player2.Hand.transform,false); 
    }
    IEnumerator DrawCardsWithDelay()
    {
        for (int i = 0; i < 10; i++)
        {       
            COCDraw();
            CRDraw();
            yield return new WaitForSeconds(0.3f); // Espera 1 segundo antes de continuar con el siguiente ciclo
        }
    }


    void PowerCounter()
    {
        //Accediendo a los contadores
        COCPowerCounter = CardDatabase.player1.TotalPower.GetComponent<TextMeshProUGUI>();
        CRPowerCounter = CardDatabase.player2.TotalPower.GetComponent<TextMeshProUGUI>();

        COCpower = 0;
        CRpower = 0;

        COCpower += SumPower(CardDatabase.player1.Melee);
        COCpower += SumPower(CardDatabase.player1.Range);
        COCpower += SumPower(CardDatabase.player1.Siege);
        CRpower += SumPower(CardDatabase.player2.Melee);
        CRpower += SumPower(CardDatabase.player2.Range);
        CRpower += SumPower(CardDatabase.player2.Siege);

        COCPowerCounter.text = COCpower.ToString();
        CRPowerCounter.text = CRpower.ToString();

        int? SumPower(GameObject zone)
        {
            int? powerSum = 0;

            // Recorrer cada hijo (carta) del objeto zone
            foreach (Transform child in zone.transform)
            {
                CardDisplay cardDisplay = child.GetComponent<CardDisplay>();

                if (cardDisplay != null)
                {
                    powerSum += cardDisplay.power; // Supongo que el poder está en una variable "power"
                }
            }

            return powerSum;
        }
    }

    void Clima()
    {
        if (ClimaZone.transform.childCount > 0)
        {
            foreach (Transform clima in ClimaZone.transform)
            {
                CardDisplay keyword = clima.GetComponent<CardDisplay>();
                Effects activate = clima.GetComponent<Effects>();
                activate.Effect(keyword.effect,clima.gameObject);
            }
        }    
    }
    void Aumento()
    {
        foreach(GameObject aumento in aumentos)
        {
            Field =aumento;
            if (Field.transform.childCount > 0)
            {
                Transform child = Field.transform.GetChild(0);
                CardDisplay keyword = child.GetComponent<CardDisplay>();
                Effects activate = child.GetComponent<Effects>();
                activate.Effect(keyword.effect,child.gameObject);
            }
        }
    }

    void CardCounter()
    {
        int COC = 0;
        int CR = 0;
        foreach(Transform card in CardDatabase.player1.Hand.transform)
        {
            COC += 1;
            if (COC > 10)
            {
                Destroy(card.gameObject,1.5f);
            }
        }
        foreach(Transform card in CardDatabase.player2.Hand.transform)
        {
            CR += 1;
            if (CR > 10)
            {
                Destroy(card.gameObject,1.5f);
            }
        }
    }

}
