using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject card;
    public GameObject COCHand;
    public GameObject CRHand;
    public GameObject ClimaZone;
    public GameObject Field;
    public List<string> aumentos = new List<string>();
    public TextMeshProUGUI COCPowerCounter;
    public TextMeshProUGUI CRPowerCounter;
    public int? COCpower;
    public int? CRpower;
    public static int? staticCOCpower;
    public static int? staticCRpower;


    // Start is called before the first frame update
    void Start()
    {
        COCHand = GameObject.Find("COCHand");
        CRHand = GameObject.Find("CRHand");
        StartCoroutine("DrawCardsWithDelay");
        ClimaZone = GameObject.Find("ClimaZone");
        aumentos.Add("COCAumento (M)");
        aumentos.Add("COCAumento (R)");
        aumentos.Add("COCAumento (S)");
        aumentos.Add("CRAumento (M)");
        aumentos.Add("CRAumento (R)");
        aumentos.Add("CRAumento (S)");
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

    public void COCDraw()
    {   
        GameObject COCcard = Instantiate(card,new Vector3(0, 0, 0), Quaternion.identity);
        COCcard.transform.SetParent(COCHand.transform,false);
    }
    public void CRDraw()
    {
        GameObject CRcard = Instantiate(card,new Vector3(0, 0, 0), Quaternion.identity);
        CRcard.transform.SetParent(CRHand.transform,false); 
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
        GameObject COCMelee = GameObject.Find("COCMelee");
        GameObject COCRange = GameObject.Find("COCRange");
        GameObject COCSiege = GameObject.Find("COCSiege");
        GameObject CRMelee = GameObject.Find("CRMelee");
        GameObject CRRange = GameObject.Find("CRRange");
        GameObject CRSiege = GameObject.Find("CRSiege");
        //Accediendo a los contadores
        GameObject goCOCPowerCounter = GameObject.Find("COCPowerCounter");
        GameObject goCRPowerCounter = GameObject.Find("CRPowerCounter");
        COCPowerCounter = goCOCPowerCounter.GetComponent<TextMeshProUGUI>();
        CRPowerCounter = goCRPowerCounter.GetComponent<TextMeshProUGUI>();

        COCpower = 0;
        CRpower = 0;

        COCpower += SumPower(COCMelee);
        COCpower += SumPower(COCRange);
        COCpower += SumPower(COCSiege);
        CRpower += SumPower(CRMelee);
        CRpower += SumPower(CRRange);
        CRpower += SumPower(CRSiege);

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
        foreach(string aumento in aumentos)
        {
            Field = GameObject.Find(aumento);
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
        foreach(Transform card in COCHand.transform)
        {
            COC += 1;
            if (COC > 10)
            {
                Destroy(card.gameObject,1.5f);
            }
        }
        foreach(Transform card in CRHand.transform)
        {
            CR += 1;
            if (CR > 10)
            {
                Destroy(card.gameObject,1.5f);
            }
        }
    }

}
