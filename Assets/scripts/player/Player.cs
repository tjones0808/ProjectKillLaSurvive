using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerState))]
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour {
    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping;
        public Vector2 Sensitivity;

        public bool LockMouse;
    }


    [SerializeField] SwatSoldier settings;
    [SerializeField] MouseInput MouseControl;
    [SerializeField] AudioController footsteps;
    [SerializeField] float minMoveThreshold;

    public PlayerAim playerAim;
    Vector3 previousPosition;

    [HideInInspector]
    public PlayerShoot playerShoot;
    

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

    InputController playerInput;
    Vector2 mouseInput;

    void Awake()
    {
        playerShoot = GetComponent<PlayerShoot>();
        playerInput = GameManager.Instance.InputController;        

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
        if (!PlayerHealth.IsAlive)
            return;

        Move();

        LookAround();
    }
    // Use this for initialization
    void Move () {

        float moveSpeed = settings.RunSpeed;

        if (playerInput.IsWalking)
            moveSpeed = settings.WalkSpeed;

        if (playerInput.IsRunning)
            moveSpeed = settings.RunSpeed;

        if (playerInput.IsProned)
            moveSpeed = settings.ProneSpeed;

        if (playerInput.IsSprinting)
            moveSpeed = settings.SprintSpeed;

        Vector2 direction = new Vector2(playerInput.Vertical * moveSpeed, playerInput.Horizontal * moveSpeed);

        //if (direction != Vector2.zero)
        //    footsteps.Play();

        MoveController.SimpleMove(transform.forward * direction.x + transform.right * direction.y );

        if (Vector3.Distance(transform.position, previousPosition) > minMoveThreshold)
            footsteps.Play();

        previousPosition = transform.position;
    }
	


    private void LookAround()
    {
        mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.MouseInput.x, 1f / MouseControl.Damping.x);
        mouseInput.y = Mathf.Lerp(mouseInput.y, playerInput.MouseInput.y, 1f / MouseControl.Damping.y);

        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
        

        playerAim.SetRotation(mouseInput.y * MouseControl.Sensitivity.y);

    }
}
