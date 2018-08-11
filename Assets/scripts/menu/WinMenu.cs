using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour {


    [SerializeField] GameObject WinMenuPanel;

    [SerializeField]
    public Button BackButton;

    private void Start()
    {
        WinMenuPanel.SetActive(false);
        GameManager.Instance.EventBus.AddListener("AllEnemiesKilled", new EventBus.EventListener()
        {
            Method = () =>
            {
                GameManager.Instance.Timer.Add(() => 
                {

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.Confined;
                    GameManager.Instance.IsPaused = true;
                    WinMenuPanel.SetActive(true);
                }, 4);
                
            },
            IsSingleShot = true
        });
        BackButton.onClick.AddListener(BackButtonClicked);
    }

    private void BackButtonClicked()
    {
        
        SceneManager.LoadScene("MainMenu");
    }
}
