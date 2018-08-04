using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
[RequireComponent(typeof(PlayerState))]
public class Player : MonoBehaviour {
    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping;
        public Vector2 Sensitivity;

        public bool LockMouse;
    }


    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float crouchSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float proneSpeed;
    [SerializeField] MouseInput MouseControl;
    [SerializeField] AudioController footsteps;
    [SerializeField] float minMoveThreshold;

    public PlayerAim playerAim;
    Vector3 previousPosition;

    [HideInInspector]
    public PlayerShoot playerShoot;
    

    MoveController m_moveController;
    public MoveController MoveController
    {
        get
        {
            if (m_moveController == null)
            {
                m_moveController = GetComponent<MoveController>();
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
    // Use this for initialization
    void Move () {

        float moveSpeed = runSpeed;

        if (playerInput.IsWalking)
            moveSpeed = walkSpeed;

        if (playerInput.IsRunning)
            moveSpeed = runSpeed;

        if (playerInput.IsProned)
            moveSpeed = proneSpeed;

        if (playerInput.IsSprinting)
            moveSpeed = sprintSpeed;

        Vector2 direction = new Vector2(playerInput.Vertical * runSpeed, playerInput.Horizontal * runSpeed);

        //if (direction != Vector2.zero)
        //    footsteps.Play();

        MoveController.Move(direction);

        if (Vector3.Distance(transform.position, previousPosition) > minMoveThreshold)
            footsteps.Play();

        previousPosition = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {

        Move();

        LookAround();
    }

    private void LookAround()
    {
        mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.MouseInput.x, 1f / MouseControl.Damping.x);
        mouseInput.y = Mathf.Lerp(mouseInput.y, playerInput.MouseInput.y, 1f / MouseControl.Damping.y);

        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);
        

        playerAim.SetRotation(mouseInput.y * MouseControl.Sensitivity.y);

    }
}
