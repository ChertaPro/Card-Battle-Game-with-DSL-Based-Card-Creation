using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocBack : MonoBehaviour
{
    public GameObject cardback;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cardback();
    }

    void Cardback()
    {
        if (CardDisplay.cocstaticcardback)
        {
            cardback.SetActive(true);
        }
        else
        {
            cardback.SetActive(false);
        }
    }
}
