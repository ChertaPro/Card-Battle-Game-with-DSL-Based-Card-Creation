using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDeckCard : MonoBehaviour
{
    public GameObject cardback;
    public GameObject cardbackfalse;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cardback.SetActive(true);
        cardbackfalse.SetActive(false);
    }
}
