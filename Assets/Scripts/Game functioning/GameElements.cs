using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class GameElements : MonoBehaviour
{
    public static List<Transform> Field (int player)
    {
        List<Transform> Field = new List<Transform>();
        if (player ==1)
        {
            if (CardDatabase.player1.Melee.transform.childCount > 0)
            {
                foreach(Transform card in CardDatabase.player1.Melee.transform)
                {
                    Field.Add(card);
                }
            }
            if (CardDatabase.player1.Range.transform.childCount > 0)
            {
                foreach(Transform card in CardDatabase.player1.Range.transform)
                {
                    Field.Add(card);
                }
            }
            if (CardDatabase.player1.Siege.transform.childCount > 0)
            {
                foreach(Transform card in CardDatabase.player1.Siege.transform)
                {
                    Field.Add(card);
                }
            }
            return Field;
        }
        
        if (player ==2)
        {
            if (CardDatabase.player2.Melee.transform.childCount > 0)
            {
                foreach(Transform card in CardDatabase.player2.Melee.transform)
                {
                    Field.Add(card);
                }
            }
            if (CardDatabase.player2.Range.transform.childCount > 0)
            {
                foreach(Transform card in CardDatabase.player2.Range.transform)
                {
                    Field.Add(card);
                }
            }
            if (CardDatabase.player2.Siege.transform.childCount > 0)
            {
                foreach(Transform card in CardDatabase.player2.Siege.transform)
                {
                    Field.Add(card);
                }
            }
            return Field;
        }
        return Field;
    }

    public static List<Transform> Board()
    {
        List<Transform> Board = new List<Transform>();
        List<Transform> Field1 = Field(1);
        List<Transform> Field2 = Field(2);

        if (Field1 != null)
        {
            foreach(Transform card in Field1)
            {
                Board.Add(card);
            }    
        }
        if(Field2 != null)
        {
            foreach (Transform card in Field2)
            {
                Board.Add(card);
            }
        }
        return Board;
    }    
}
