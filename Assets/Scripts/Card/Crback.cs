using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crback : MonoBehaviour
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
        if (CardDisplay.crstaticcardback)
        {
            cardback.SetActive(true);
        }
        else if (!CardDisplay.crstaticcardback)
        {
            cardback.SetActive(false);
        }
    }
}
