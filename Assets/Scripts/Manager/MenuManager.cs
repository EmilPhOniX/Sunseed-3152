using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnApplicationQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}