using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class LeaderEvents : MonoBehaviour
{
    public GameObject Playercard;
    public GameObject COCLeader;
    public GameObject CRLeader;
    public GameObject Cardstats;
    public TextMeshProUGUI Powerstat;
    public Image Cardimage;
    
    public static bool COCLeaderEffect;
    public static bool CRLeaderEffect;
    
    
    void Start()
    {
        COCLeaderEffect = true;
        CRLeaderEffect = true;
        
    }

    public void OnClick()
    {
        COCLeader = GameObject.Find("COCLeader");
        CRLeader = GameObject.Find("CRLeader");
        
        if (gameObject.transform.parent == COCLeader.transform && TurnSystem.turn == 1 && COCLeaderEffect == true)
        {
            Playercard = COCLeader.transform.GetChild(0).gameObject;
            CardDisplay keyword = Playercard.GetComponent<CardDisplay>();
            Effects activate = Playercard.GetComponent<Effects>();
            TurnSystem.turn = 0;
            activate.Effect(keyword.effect,Playercard);
            COCLeaderEffect = false;
        }
        if (gameObject.transform.parent == CRLeader.transform && TurnSystem.turn == 0 && CRLeaderEffect == true)
        {
            Playercard = CRLeader.transform.GetChild(0).gameObject;
            CardDisplay keyword = Playercard.GetComponent<CardDisplay>();
            Effects activate = Playercard.GetComponent<Effects>();
            TurnSystem.turn = 1;
            activate.Effect(keyword.effect,Playercard);
            CRLeaderEffect = false;
        }
        
    }

    public void HoverEnter()
    {
        Cardstats = GameObject.Find("Stats");
        Cardimage = Cardstats.GetComponent<Image>();
        GameObject hideObject = Cardstats.transform.Find("Hide")?.gameObject;
        Powerstat = hideObject.transform.Find("Power")?.GetComponent<TextMeshProUGUI>();
        CardDisplay stats= gameObject.GetComponent<CardDisplay>();
        Cardimage.sprite = stats.spriteimage;
        Powerstat.text = stats.power.ToString();
    }
    
}
