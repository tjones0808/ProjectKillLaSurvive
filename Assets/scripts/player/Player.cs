using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
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

    private Crosshair m_crosshair;
    public Crosshair Crosshair
    {
        get {
            if(m_crosshair == null)
                m_crosshair = GetComponentInChildren<Crosshair>();

            return m_crosshair;
        }
    }

    InputController playerInput;
    Vector2 mouseInput;

    void Awake()
    {
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
        MoveController.Move(direction);
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

        Crosshair.LookHeight(mouseInput.y * MouseControl.Sensitivity.y);
    }
}
