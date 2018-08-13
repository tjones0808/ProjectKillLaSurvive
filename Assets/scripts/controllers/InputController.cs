using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    [System.Serializable]
    public class InputState
    {
        public float Vertical;
        public float Horizontal;
        public bool Fire1;
        public bool Fire2;
        public bool Reload;
        public bool IsWalking;
        public bool IsRunning;
        public bool IsCrouched;
        public bool IsProned;
        public bool IsSprinting;
        public bool CoverToggle;
        public float AimAngle;
        public bool IsAiming;
        public bool IsInCover;
    }

    public float Vertical
    {
        get { return State.Vertical; }
    }
    public float Horizontal
    {
        get { return State.Horizontal; }
    }
    public bool Fire1
    {
        get { return State.Fire1; }
    }
    public bool Fire2
    {
        get { return State.Fire2; }
    }
    public bool Reload
    {
        get { return State.Reload; }
    }
    public bool IsWalking
    {
        get { return State.IsWalking; }
    }
    public bool IsSprinting
    {
        get { return State.IsSprinting; }
    }
    public bool IsCrouched
    {
        get { return State.IsCrouched; }
    }
    public bool CoverToggle
    {
        get { return State.CoverToggle; }
    }
    
    
    public Vector2 MouseInput;
    public float MouseWheelUp;
    public float MouseWheelDown;
   

    public bool Escape;
    public bool ChangeCamera;
    public InputState State;

    private void Start()
    {
        State = new InputState();
    }

    void Update()
    {
        State.Vertical = Input.GetAxis("Vertical");
        State.Horizontal = Input.GetAxis("Horizontal");       
        State.Fire1 = Input.GetButton("Fire1");
        State.Fire2 = Input.GetButton("Fire2");
        State.Reload = Input.GetKey(KeyCode.R);
        State.CoverToggle = Input.GetKeyDown(KeyCode.F);
        State.IsWalking = Input.GetKey(KeyCode.LeftAlt);
        State.IsSprinting = Input.GetKey(KeyCode.LeftShift);        
        State.IsCrouched = Input.GetKey(KeyCode.C);

        ChangeCamera = Input.GetKey(KeyCode.V);
        Escape = Input.GetKey(KeyCode.Escape);
        MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        MouseWheelUp = Input.GetAxis("Mouse ScrollWheel");
        MouseWheelDown = Input.GetAxis("Mouse ScrollWheel");

    }

}


