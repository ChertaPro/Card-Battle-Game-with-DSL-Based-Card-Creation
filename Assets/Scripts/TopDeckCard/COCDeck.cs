using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class COCDeck : MonoBehaviour
{
    public GameObject top1, top2, top3;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        TopDeck();  
    }
    
//**----------------------------------------------    
    void TopDeck()
    {
        if(CardDatabase.COCDeck.Count<20)
        {
            top1.SetActive(false);
        }
        if(CardDatabase.COCDeck.Count<8)
        {
            top2.SetActive(false);
        }
        if(CardDatabase.COCDeck.Count<1)
        {
            top3.SetActive(false);
        }
    }
}
