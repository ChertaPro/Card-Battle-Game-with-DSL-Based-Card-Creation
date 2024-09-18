using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompileButton : MonoBehaviour
{
    public GameObject input;
    private TMP_InputField inputField;
    public void Compile()
    {
        input = GameObject.Find("CodigoDSL");
        inputField = input.GetComponent<TMP_InputField>();
        DSL.Compile(inputField.text);
    }

    public void Exit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
