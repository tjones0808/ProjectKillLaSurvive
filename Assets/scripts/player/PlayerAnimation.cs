using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private Animator animator;
    public float Vertical;
    public float Horizontal;
    public bool IsWalking;
    public bool IsCrouched;
    public bool IsSprinting;
    public float AimAngle;
    public bool IsAiming;
    public bool IsInCover;
    
    private PlayerAim m_playerAim;
    private PlayerAim PlayerAim
    {
        get
        {
            if (m_playerAim == null && GameManager.Instance.LocalPlayer != null)
                m_playerAim = GameManager.Instance.LocalPlayer.playerAim;

            return m_playerAim;
        }
    }

    private Player m_Player;
    private Player Player
    {
        get
        {
            if (m_Player == null)
                m_Player = GetComponent<Player>();
            return m_Player;
        }
    }


    void GetLocalPlayerInput()
    {
        Vertical = Player.InputState.Vertical;
        Horizontal = Player.InputState.Horizontal;

        IsWalking = GameManager.Instance.LocalPlayer.PlayerState.MoveState == PlayerState.EMoveState.WALKING;
        IsSprinting = Player.InputState.IsSprinting;
        IsCrouched = Player.InputState.IsCrouched;

        AimAngle = PlayerAim.GetAngle();
        IsAiming = GameManager.Instance.LocalPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMING ||
            GameManager.Instance.LocalPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMEDFIRING;
        IsInCover = GameManager.Instance.LocalPlayer.PlayerState.MoveState == PlayerState.EMoveState.COVER ? true : false;
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    

    private void Update()
    {
        if (GameManager.Instance.IsPaused)
            return;

        if (Player.IsLocalPlayer)
            GetLocalPlayerInput();

        animator.SetFloat("Vertical", Vertical);
        animator.SetFloat("Horizontal", Horizontal);

        animator.SetBool("IsWalking", IsWalking);
        animator.SetBool("IsSprinting",IsSprinting);
        animator.SetBool("IsCrouched", IsCrouched);

        animator.SetFloat("AimAngle", AimAngle);

        animator.SetBool("IsAiming", IsAiming);

        animator.SetBool("IsInCover", IsInCover);

    }
}
