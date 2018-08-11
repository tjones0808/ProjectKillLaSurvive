using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerShoot : WeaponController {

    bool IsPlayerAlive;
    private void Start()
    {
        IsPlayerAlive = true;
        GetComponent<Player>().PlayerHealth.OnDeath += PlayerHealth_OnDeath;
    }

    private void PlayerHealth_OnDeath()
    {
        IsPlayerAlive = false;
    }

    private void Update()
    {
        if(!IsPlayerAlive || GameManager.Instance.IsPaused)
            return;

        if (GameManager.Instance.InputController.MouseWheelDown < 0)
            SwitchWeapon(-1);
        else if (GameManager.Instance.InputController.MouseWheelUp > 0)
            SwitchWeapon(1);

        if (GameManager.Instance.LocalPlayer.PlayerState.MoveState == PlayerState.EMoveState.SPRINTING)
            return;

        if (!CanFire)
            return;

        if (GameManager.Instance.InputController.Fire1)
        {
            ActiveWeapon.Fire();
        }


        if (GameManager.Instance.InputController.Reload)
            ActiveWeapon.Reload();
    }

}
