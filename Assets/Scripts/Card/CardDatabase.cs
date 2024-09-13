using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static List<Card> COCDeck = new List<Card>();
    public static List<Card> CRDeck = new List<Card>();
    public static List<Card> COCbackup = new List<Card>();
    public static List<Card> CRbackup = new List<Card>();
    public static List<Card> Leaders = new List<Card>();    
    
    public static Player player1;
    public static Player player2;

    void Awake()
    {
        //Leaders
        Leaders.Add(new Card(0,1,"Reina Arquera","Lider","COC",null, null,"mantener",Resources.Load<Sprite>("0-Reina Arquera"),true,true));
        Leaders.Add(new Card(1,2,"Rey","Lider","CR",null,null,"roba",Resources.Load<Sprite>("17-Rey"),true,true));
        //COC
        COCDeck.Add(new Card(1,1,"Pekka","Oro","COC",9,new List<string> { "Melee"},"+poder",Resources.Load<Sprite>("1-Pekka"),true,true));
        COCDeck.Add(new Card(2,1,"Gran Centinela","Oro","COC",7,new List<string> { "Ranged"},"aumento",Resources.Load<Sprite>("2-Gran Centinela"),true,true));
        COCDeck.Add(new Card(3,1,"Rey Barbaro","Oro","COC",8,new List<string> { "Melee"},"roba",Resources.Load<Sprite>("3-Rey Barbaro"),true,true));
        COCDeck.Add(new Card(4,1,"Lanzarrocas","Plata","COC",4,new List<string> { "Ranged"},"roba",Resources.Load<Sprite>("4-Lanzarrocas"),true,true));
        COCDeck.Add(new Card(5,1,"Lanzarrocas","Plata","COC",4,new List<string> { "Ranged"},"roba",Resources.Load<Sprite>("4-Lanzarrocas"),true,true));
        COCDeck.Add(new Card(6,1,"Lanzarrocas","Plata","COC",4,new List<string> { "Ranged"},"roba",Resources.Load<Sprite>("4-Lanzarrocas"),true,true));
        COCDeck.Add(new Card(7,1,"Montapuercos","Plata","Neutral",6,new List<string> { "Melee"},"-poder",Resources.Load<Sprite>("5-Montapuercos"),true,true));//*! Neutral
        COCDeck.Add(new Card(8,1,"Montapuercos","Plata","Neutral",6,new List<string> { "Melee"},"-poder",Resources.Load<Sprite>("5-Montapuercos"),true,true));//*! Neutral
        COCDeck.Add(new Card(9,1,"Montapuercos","Plata","Neutral",6,new List<string> { "Melee"},"-poder",Resources.Load<Sprite>("5-Montapuercos"),true,true));//*! Neutral
        COCDeck.Add(new Card(10,1,"Bruja","Plata","COC",6,new List<string> { "Ranged"},"+poder",Resources.Load<Sprite>("6-Bruja"),true,true));
        COCDeck.Add(new Card(11,1,"Bruja","Plata","COC",6,new List<string> { "Ranged"},"+poder",Resources.Load<Sprite>("6-Bruja"),true,true));
        COCDeck.Add(new Card(12,1,"Bruja","Plata","COC",6,new List<string> { "Ranged"},"+poder",Resources.Load<Sprite>("6-Bruja"),true,true));
        COCDeck.Add(new Card(13,1,"Lanzatroncos","Plata","COC",4,new List<string> { "Siege"},"clima",Resources.Load<Sprite>("7-Lanzatroncos"),true,true));
        COCDeck.Add(new Card(14,1,"Lanzatroncos","Plata","COC",4,new List<string> { "Siege"},"clima",Resources.Load<Sprite>("7-Lanzatroncos"),true,true));
        COCDeck.Add(new Card(15,1,"Lanzatroncos","Plata","COC",4,new List<string> { "Siege"},"clima",Resources.Load<Sprite>("7-Lanzatroncos"),true,true));
        COCDeck.Add(new Card(16,1,"Arrojapiedras","Plata","COC",7,new List<string> { "Siege"},"-poder",Resources.Load<Sprite>("8-ArrojaPiedras"),true,true));
        COCDeck.Add(new Card(17,1,"Arrojapiedras","Plata","COC",7,new List<string> { "Siege"},"-poder",Resources.Load<Sprite>("8-ArrojaPiedras"),true,true));
        COCDeck.Add(new Card(18,1,"Arrojapiedras","Plata","COC",7,new List<string> { "Siege"},"-poder",Resources.Load<Sprite>("8-ArrojaPiedras"),true,true));
        COCDeck.Add(new Card(19,1,"Hechizo de Rayo","Clima","Neutral",null,null,"range",Resources.Load<Sprite>("9-Hechizo de Rayo"),true,true));//*! Neutral
        COCDeck.Add(new Card(20,1,"Montepuerco","Clima","Neutral",null,null,"melee",Resources.Load<Sprite>("10-Montepuerco"),true,true));//*! Neutral
        COCDeck.Add(new Card(21,1,"Hechizo de furia","Aumento","COC",null,null,"bonus",Resources.Load<Sprite>("11-Hechizo de Furia"),true,true));
        COCDeck.Add(new Card(22,1,"Taller del constructor","Aumento","Neutral",null,null,"bonus",Resources.Load<Sprite>("12-Taller del Constructor"),true,true));//*! Neutral
        COCDeck.Add(new Card(23,1,"Controlador aereo","Despeje","COC",null,null,"despeje",Resources.Load<Sprite>("13-Controlador Aereo"),true,true));
        COCDeck.Add(new Card(24,1,"Tornado","Despeje","Neutral",null,null,"despeje",Resources.Load<Sprite>("14-Tornado"),true,true));//*! Neutral
        COCDeck.Add(new Card(25,1,"Duende","Señuelo","Neutral",0,null,"señuelo",Resources.Load<Sprite>("15-Duende"),true,true));//*! Neutral
        COCDeck.Add(new Card(26,1,"Esbirro","Señuelo","Neutral",0,null,"señuelo",Resources.Load<Sprite>("16-Esbirro"),true,true));//*! Neutral
        //CR
        CRDeck.Add(new Card(1,2,"Caballero dorado","Oro","CR",8,new List<string> { "Melee"},"+poder",Resources.Load<Sprite>("18-Caballero Dorado"),true,true));
        CRDeck.Add(new Card(2,2,"Gigante electrico","Oro","CR",10,new List<string> { "Melee"},"clima",Resources.Load<Sprite>("19-Gigante Electrico"),true,true));
        CRDeck.Add(new Card(3,2,"Gigante noble","Oro","CR",7,new List<string> { "Ranged"},"roba",Resources.Load<Sprite>("20-Gigante Noble"),true,true));
        CRDeck.Add(new Card(4,2,"Lanzafuegos","Plata","CR",5,new List<string> { "Ranged"},"roba",Resources.Load<Sprite>("21-Lanzafuegos"),true,true));
        CRDeck.Add(new Card(5,2,"Lanzafuegos","Plata","CR",5,new List<string> { "Ranged"},"roba",Resources.Load<Sprite>("21-Lanzafuegos"),true,true));
        CRDeck.Add(new Card(6,2,"Lanzafuegos","Plata","CR",5,new List<string> { "Ranged"},"roba",Resources.Load<Sprite>("21-Lanzafuegos"),true,true));
        CRDeck.Add(new Card(7,2,"Montapuercos","Plata","Neutral",6,new List<string> { "Melee"},"-poder",Resources.Load<Sprite>("5-Montapuercos"),true,true));//*! Neutral
        CRDeck.Add(new Card(8,2,"Montapuercos","Plata","Neutral",6,new List<string> { "Melee"},"-poder",Resources.Load<Sprite>("5-Montapuercos"),true,true));//*! Neutral
        CRDeck.Add(new Card(9,2,"Montapuercos","Plata","Neutral",6,new List<string> { "Melee"},"-poder",Resources.Load<Sprite>("5-Montapuercos"),true,true));//*! Neutral
        CRDeck.Add(new Card(10,2,"Globo bombastico","Plata","CR",6,new List<string> { "Siege"},"+poder",Resources.Load<Sprite>("22-Globo Bombástico"),true,true));
        CRDeck.Add(new Card(11,2,"Globo bombastico","Plata","CR",6,new List<string> { "Siege"},"+poder",Resources.Load<Sprite>("22-Globo Bombástico"),true,true));
        CRDeck.Add(new Card(12,2,"Globo bombastico","Plata","CR",6,new List<string> { "Siege"},"+poder",Resources.Load<Sprite>("22-Globo Bombástico"),true,true));
        CRDeck.Add(new Card(13,2,"Mosquetera","Plata","CR",4,new List<string> { "Ranged"},"aumento",Resources.Load<Sprite>("23-Mosquetera"),true,true));
        CRDeck.Add(new Card(14,2,"Mosquetera","Plata","CR",4,new List<string> { "Ranged"},"aumento",Resources.Load<Sprite>("23-Mosquetera"),true,true));
        CRDeck.Add(new Card(15,2,"Mosquetera","Plata","CR",4,new List<string> { "Ranged"},"aumento",Resources.Load<Sprite>("23-Mosquetera"),true,true));
        CRDeck.Add(new Card(16,2,"Cañon con ruedas","Plata","CR",5,new List<string> {"Ranged", "Siege"},"-poder",Resources.Load<Sprite>("24-Cañon con Ruedas"),true,true));
        CRDeck.Add(new Card(17,2,"Cañon con ruedas","Plata","CR",5,new List<string> {"Ranged", "Siege"},"-poder",Resources.Load<Sprite>("24-Cañon con Ruedas"),true,true));
        CRDeck.Add(new Card(18,2,"Cañon con ruedas","Plata","CR",5,new List<string> {"Ranged", "Siege"},"-poder",Resources.Load<Sprite>("24-Cañon con Ruedas"),true,true));
        CRDeck.Add(new Card(19,2,"Hechizo de Rayo","Clima","Neutral",null,null,"range",Resources.Load<Sprite>("9-Hechizo de Rayo"),true,true));//*! Neutral
        CRDeck.Add(new Card(20,2,"Montepuerco","Clima","Neutral",null,null,"melee",Resources.Load<Sprite>("10-Montepuerco"),true,true));//*! Neutral
        CRDeck.Add(new Card(21,2,"Arena Real","Aumento","CR",null,null,"bonus",Resources.Load<Sprite>("25-Arena real"),true,true));
        CRDeck.Add(new Card(22,2,"Taller del constructor","Aumento","Neutral",null,null,"bonus",Resources.Load<Sprite>("12-Taller del Constructor"),true,true));//*! Neutral
        CRDeck.Add(new Card(23,2,"Espiritu de fuego","Despeje","CR",null,null,"despeje",Resources.Load<Sprite>("26-Espiritu de fuego"),true,true));        
        CRDeck.Add(new Card(24,2,"Tornado","Despeje","Neutral",null,null,"despeje",Resources.Load<Sprite>("14-Tornado"),true,true));//*! Neutral
        CRDeck.Add(new Card(25,2,"Duende","Señuelo","Neutral",0,null,"señuelo",Resources.Load<Sprite>("15-Duende"),true,true));//*! Neutral
        CRDeck.Add(new Card(26,2,"Esbirro","Señuelo","Neutral",0,null,"señuelo",Resources.Load<Sprite>("16-Esbirro"),true,true));//*! Neutral
        COCbackup = COCDeck;
        CRbackup = CRDeck;
        player1 = new Player(1,GameObject.Find("COCLeader"),GameObject.Find("COCHand"),CardDatabase.COCDeck,GameObject.Find("COCMelee"),GameObject.Find("COCRange"), GameObject.Find("COCSiege"), GameObject.Find("COCAumento (M)"), GameObject.Find("COCAumento (R)"), GameObject.Find("COCAumento (S)"), GameObject.Find("ClimaZone"), GameObject.Find("COCGraveyard"), GameObject.Find("COCPowerCounter"),GameObject.Find("COCWins"));
        player2 = new Player(2,GameObject.Find("CRLeader"),GameObject.Find("CRHand"),CardDatabase.CRDeck,GameObject.Find("CRMelee"),GameObject.Find("CRRange"), GameObject.Find("CRSiege"), GameObject.Find("CRAumento (M)"), GameObject.Find("CRAumento (R)"), GameObject.Find("CRAumento (S)"), GameObject.Find("ClimaZone"), GameObject.Find("CRGraveyard"), GameObject.Find("CRPowerCounter"),GameObject.Find("CRWins"));

    }

    void Start()
    {
        
    }

    

}
