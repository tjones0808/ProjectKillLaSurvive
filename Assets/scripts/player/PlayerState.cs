using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    bool isInCover = false;

    public enum EMoveState
    {
        WALKING,
        RUNNING,
        CROUCHING,
        SPRINTING,
        COVER
    }

    public enum EWeaponState
    {
        IDLE,
        FIRING,
        AIMING,
        AIMEDFIRING
    }

    public EMoveState MoveState;
    public EWeaponState WeaponState;

    private InputController m_InputController;
    public InputController InputController
    {
        get
        {
            if (m_InputController == null)
                m_InputController = GameManager.Instance.InputController;

            return m_InputController;
        }
    }

    private void Awake()
    {
        GameManager.Instance.EventBus.AddListener("CoverToggle", new EventBus.EventListener()
        {
            Method = ToggleCover
        });
    }

    void ToggleCover()
    {
        isInCover = !isInCover;
    }

    private void Update()
    {
        SetMoveState();
        SetWeaponState();
    }

    void SetWeaponState()
    {
        WeaponState = EWeaponState.IDLE;

        if (InputController.Fire1)
        {
            WeaponState = EWeaponState.FIRING;
        }

        if (InputController.Fire1 && InputController.Fire2)
        {
            WeaponState = EWeaponState.AIMEDFIRING;
        }

        if (InputController.Fire2)
        {
            WeaponState = EWeaponState.AIMING;
        }
        
    }

    void SetMoveState()
    {
        MoveState = EMoveState.RUNNING;

        if (InputController.IsSprinting)
            MoveState = EMoveState.SPRINTING;

        if (InputController.IsWalking)
            MoveState = EMoveState.WALKING;

        if (InputController.IsCrouched)
            MoveState = EMoveState.CROUCHING;

        if (isInCover)
            MoveState = EMoveState.COVER;

    }


}
