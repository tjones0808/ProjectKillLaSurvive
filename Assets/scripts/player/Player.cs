using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping;
        public Vector2 Sensitivity;

        public bool LockMouse;
    }

    [SerializeField]public SwatSoldier Settings;
    [SerializeField] MouseInput MouseControl;
    [SerializeField] AudioController footsteps;
    [SerializeField] float minMoveThreshold;

    public PlayerAim playerAim;
    public bool IsLocalPlayer;
    Vector3 previousPosition;

    PlayerShoot m_PlayerShoot;
    public PlayerShoot PlayerShoot
    {
        get
        {
            if (m_PlayerShoot == null)
                m_PlayerShoot = GetComponent<PlayerShoot>();
            return m_PlayerShoot;
        }
    }

    WeaponController m_WeaponController;
    public WeaponController WeaponControllerr
    {
        get
        {
            if (m_WeaponController == null)
                m_WeaponController = GetComponent<WeaponController>();
            return m_WeaponController;
        }
    }


    CharacterController m_moveController;
    public CharacterController MoveController
    {
        get
        {
            if (m_moveController == null)
            {
                m_moveController = GetComponent<CharacterController>();
            }
            return m_moveController;
        }
        set { }
    }

    private InputController.InputState m_InputState;
    public InputController.InputState InputState
    {
        get
        {
            if (m_InputState == null)
                m_InputState = GameManager.Instance.InputController.State;
            return m_InputState;
        }
    }

    private PlayerState m_playerState;
    public PlayerState PlayerState
    {
        get
        {
            if (m_playerState == null)
                m_playerState = GetComponent<PlayerState>();

            return m_playerState;
        }
    }

    private PlayerHealth m_playerHealth;
    public PlayerHealth PlayerHealth
    {
        get
        {
            if (m_playerHealth == null)
                m_playerHealth = GetComponent<PlayerHealth>();

            return m_playerHealth;
        }
    }

    Vector2 mouseInput;

    void Awake()
    {
        //if (GameManager.Instance.IsNetworkGame)
            SetAsLocalPlayer();


    }

    public void SetAsLocalPlayer()
    {
        IsLocalPlayer = true;

        GameManager.Instance.LocalPlayer = this;

        if (MouseControl.LockMouse)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!PlayerHealth.IsAlive || GameManager.Instance.IsPaused)
            return;

        if (IsLocalPlayer)
        {
            if (!GameManager.Instance.IsNetworkGame)
            {
                Move();

            }


            LookAround();

        }

    }

    public void SetInputController(InputController.InputState state)
    {
        m_InputState = state;
    }

    private void Move()
    {
        if (InputState == null)
            return;

        Move(InputState.Horizontal, InputState.Vertical);


    }

    public void Move(float horizontal, float vertical)
    {
        float moveSpeed = Settings.RunSpeed;

        if (InputState.IsWalking)
            moveSpeed = Settings.WalkSpeed;

        //if (playerInput.IsProned)
        //    moveSpeed = settings.ProneSpeed;

        if (InputState.IsSprinting)
            moveSpeed = Settings.SprintSpeed;

        if (PlayerState.MoveState == PlayerState.EMoveState.COVER)
            moveSpeed = Settings.WalkSpeed;

        Vector2 direction = new Vector2(vertical * moveSpeed, horizontal * moveSpeed);

        //if (direction != Vector2.zero)
        //    footsteps.Play();

        MoveController.SimpleMove(transform.forward * direction.x + transform.right * direction.y);

        if (Vector3.Distance(transform.position, previousPosition) > minMoveThreshold)
            footsteps.Play();

        previousPosition = transform.position;
    }

    [ContextMenu("Respawn")]
    void Respawn()
    {
        // add spawnpointcontroller
    }



    private void LookAround()
    {
        mouseInput.x = Mathf.Lerp(mouseInput.x, GameManager.Instance.InputController.MouseInput.x, 1f / MouseControl.Damping.x);
        mouseInput.y = Mathf.Lerp(mouseInput.y, GameManager.Instance.InputController.MouseInput.y, 1f / MouseControl.Damping.y);

        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);


        playerAim.SetRotation(mouseInput.y * MouseControl.Sensitivity.y);

    }
}
