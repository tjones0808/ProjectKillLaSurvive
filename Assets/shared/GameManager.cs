using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{

    public event System.Action<Player> OnLocalPlayerJoined;
    private GameObject gameObject;

    public bool IsPaused;

    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GameManager();
                m_Instance.gameObject = new GameObject("_gameManager");
                m_Instance.gameObject.AddComponent<InputController>();
                m_Instance.gameObject.AddComponent<Timer>();
                m_Instance.gameObject.AddComponent<Respawner>();
            }
            return m_Instance;
        }
        set
        { }
    }

    private Respawner m_Respawner;
    public Respawner Respawner
    {
        get
        {
            if (m_Respawner == null)
                m_Respawner = gameObject.GetComponent<Respawner>();

            return m_Respawner;
        }
    }

    private Timer m_timer;
    public Timer Timer
    {
        get
        {
            if (m_timer == null)
                m_timer = gameObject.GetComponent<Timer>();

            return m_timer;
        }
    }

    private EventBus m_EventBus;
    public EventBus EventBus
    {
        get
        {
            if (m_EventBus == null)
                m_EventBus = new EventBus();

            return m_EventBus;
        }
    }


    private InputController m_inputController;
    public InputController InputController
    {
        get
        {
            if (m_inputController == null)
            {
                m_inputController = gameObject.GetComponent<InputController>();
            }

            return m_inputController;
        }
        set { }
    }

    private Player m_localPlayer;
    public Player LocalPlayer
    {
        get
        {
            return m_localPlayer;
        }
        set
        {
            m_localPlayer = value;
            if (OnLocalPlayerJoined != null)
                OnLocalPlayerJoined(m_localPlayer);
        }
    }
}
