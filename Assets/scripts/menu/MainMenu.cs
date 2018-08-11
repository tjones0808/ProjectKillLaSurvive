using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {


    public string levelName;
    [SerializeField]
    public Button StartGameButton;
    [SerializeField]
    public Button QuitGameButton;

    private void Start()
    {
        StartGameButton.onClick.AddListener(() => 
        {
            StartGame(levelName);
        });

        QuitGameButton.onClick.AddListener(() =>
        {
            QuitGame();
        });
    }
    public void StartGame(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
