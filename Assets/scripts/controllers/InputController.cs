﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {
    
    public float Vertical;
    public float Horizontal;

    public float RightStickVertical;
    public float RightStickHorizontal;
    public Vector2 MouseInput;
    public bool Fire1;
    public bool Fire2;
    public bool Reload;
    public bool IsWalking;
    public bool IsRunning;
    public bool IsCrouched;
    public bool IsProned;
    public bool IsSprinting;
    public float MouseWheelUp;
    public float MouseWheelDown;
    public bool CoverToggle;

    public bool Escape;

    void Update()
    {
        Vertical = Input.GetAxis("Vertical");
        Horizontal = Input.GetAxis("Horizontal");
        MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Fire1 = Input.GetButton("Fire1");
        Fire2 = Input.GetButton("Fire2");
        Reload = Input.GetKey(KeyCode.R);
        CoverToggle = Input.GetKeyDown(KeyCode.F);
        IsWalking = Input.GetKey(KeyCode.LeftAlt);
        IsSprinting = Input.GetKey(KeyCode.LeftShift);
        Escape = Input.GetKey(KeyCode.Escape);
        IsCrouched = Input.GetKey(KeyCode.C);
        MouseWheelUp = Input.GetAxis("Mouse ScrollWheel");
        MouseWheelDown = Input.GetAxis("Mouse ScrollWheel");

    }

}


