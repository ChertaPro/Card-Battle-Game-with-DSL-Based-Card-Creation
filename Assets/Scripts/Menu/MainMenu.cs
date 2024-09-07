using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        StartCoroutine("PlayGame");
    }
    public void CreateCard()
    {
        StartCoroutine("Create");
    }
    public void Exit()
    {
        Application.Quit();
    }
    IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator Create()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
