using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static List<Card> cards = new List<Card>();
    


    void Awake()
    {
        cards.Add(new Card(0,"Reina Arquera","Lider",null,"COC", null,"mantener",Resources.Load<Sprite>("0-Reina Arquera"),true,true));
        cards.Add(new Card(1,"Pekka","Oro",new List<string> { "Melee"},"COC",9,"+poder",Resources.Load<Sprite>("1-Pekka"),true,true));
        cards.Add(new Card(2,"Gran Centinela","Oro",new List<string> { "Ranged"},"COC",7,"aumento",Resources.Load<Sprite>("2-Gran Centinela"),true,true));
        cards.Add(new Card(3,"Rey Barbaro","Oro",new List<string> { "Melee"},"COC",8,"roba",Resources.Load<Sprite>("3-Rey Barbaro"),true,true));
        cards.Add(new Card(4,"Lanzarrocas","Plata",new List<string> { "Ranged"},"COC",4,"roba",Resources.Load<Sprite>("4-Lanzarrocas"),true,true));
        cards.Add(new Card(5,"Montapuercos","Plata",new List<string> { "Melee"},"Neutral",6,"-poder",Resources.Load<Sprite>("5-Montapuercos"),true,true));//*! Neutral
        cards.Add(new Card(6,"Bruja","Plata",new List<string> { "Ranged"},"COC",6,"+poder",Resources.Load<Sprite>("6-Bruja"),true,true));
        cards.Add(new Card(7,"Lanzatroncos","Plata",new List<string> { "Siege"},"COC",4,"clima",Resources.Load<Sprite>("7-Lanzatroncos"),true,true));
        cards.Add(new Card(8,"Arrojapiedras","Plata",new List<string> { "Siege"},"COC",7,"-poder",Resources.Load<Sprite>("8-ArrojaPiedras"),true,true));
        cards.Add(new Card(9,"Hechizo de Rayo","Clima",null,"Neutral",null,"range",Resources.Load<Sprite>("9-Hechizo de Rayo"),true,true));//*! Neutral
        cards.Add(new Card(10,"Montepuerco","Clima",null,"Neutral",null,"melee",Resources.Load<Sprite>("10-Montepuerco"),true,true));//*! Neutral
        cards.Add(new Card(11,"Hechizo de furia","Aumento",null,"COC",null,"bonus",Resources.Load<Sprite>("11-Hechizo de Furia"),true,true));
        cards.Add(new Card(12,"Taller del constructor","Aumento",null,"Neutral",null,"bonus",Resources.Load<Sprite>("12-Taller del Constructor"),true,true));//*! Neutral
        cards.Add(new Card(13,"Controlador aereo","Despeje",null,"COC",null,"despeje",Resources.Load<Sprite>("13-Controlador Aereo"),true,true));
        cards.Add(new Card(14,"Tornado","Despeje",null,"Neutral",null,"despeje",Resources.Load<Sprite>("14-Tornado"),true,true));//*! Neutral
        cards.Add(new Card(15,"Duende","Señuelo",null,"Neutral",0,"señuelo",Resources.Load<Sprite>("15-Duende"),true,true));//*! Neutral
        cards.Add(new Card(16,"Esbirro","Señuelo",null,"Neutral",0,"señuelo",Resources.Load<Sprite>("16-Esbirro"),true,true));//*! Neutral
        cards.Add(new Card(17,"Rey","Lider",null,"CR",null,"roba",Resources.Load<Sprite>("17-Rey"),true,true));
        cards.Add(new Card(18,"Caballero dorado","Oro",new List<string> { "Melee"},"CR",8,"+poder",Resources.Load<Sprite>("18-Caballero Dorado"),true,true));
        cards.Add(new Card(19,"Gigante electrico","Oro",new List<string> { "Melee"},"CR",10,"clima",Resources.Load<Sprite>("19-Gigante Electrico"),true,true));
        cards.Add(new Card(20,"Gigante noble","Oro",new List<string> { "Ranged"},"CR",7,"roba",Resources.Load<Sprite>("20-Gigante Noble"),true,true));
        cards.Add(new Card(21,"Lanzafuegos","Plata",new List<string> { "Ranged"},"CR",5,"roba",Resources.Load<Sprite>("21-Lanzafuegos"),true,true));
        cards.Add(new Card(22,"Globo bombastico","Plata",new List<string> { "Siege"},"CR",6,"+poder",Resources.Load<Sprite>("22-Globo Bombástico"),true,true));
        cards.Add(new Card(23,"Mosquetera","Plata",new List<string> { "Ranged"},"CR",4,"aumento",Resources.Load<Sprite>("23-Mosquetera"),true,true));
        cards.Add(new Card(24,"Cañon con ruedas","Plata",new List<string> {"Ranged", "Siege"},"CR",5,"-poder",Resources.Load<Sprite>("24-Cañon con Ruedas"),true,true));
        cards.Add(new Card(25,"Arena Real","Aumento",null,"CR",null,"bonus",Resources.Load<Sprite>("25-Arena real"),true,true));
        cards.Add(new Card(26,"Espiritu de fuego","Despeje",null,"CR",null,"despeje",Resources.Load<Sprite>("26-Espiritu de fuego"),true,true));
    }

    void Start()
    {
        
    }

    

}
