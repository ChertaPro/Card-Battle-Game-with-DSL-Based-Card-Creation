using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompileButton : MonoBehaviour
{
    public void Compile()
    {
        Debug.Log("AAAAAAAAAAAAAAAA");
    }

    public void Exit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
