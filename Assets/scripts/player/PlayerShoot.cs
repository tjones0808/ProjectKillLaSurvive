using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    [SerializeField] float weaponSwitchTime;

    Shooter[] weapons;
    public Shooter activeWeapon;

    int currentWeaponIndex;
    bool canFire;
    Transform weaponHolster;

    public event System.Action<Shooter> OnWeaponSwitch;


    private void Awake()
    {
        weaponHolster = transform.FindChild("Weapons");
        weapons = transform.FindChild("Weapons").GetComponentsInChildren<Shooter>();
        

        if (weapons.Length > 0)
            Equip(0);

        canFire = true;

    }

    private void Update()
    {
        if (GameManager.Instance.InputController.MouseWheelDown < 0)
            SwitchWeapon(-1);
        else if (GameManager.Instance.InputController.MouseWheelUp > 0)
            SwitchWeapon(1);

        if (GameManager.Instance.LocalPlayer.PlayerState.MoveState == PlayerState.EMoveState.SPRINTING)
            return;

        if (!canFire)
            return;

        if (GameManager.Instance.InputController.Fire1)
        {
            activeWeapon.Fire();
        }
    }

    void DeactivateWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {            
            weapons[i].gameObject.SetActive(false);
            weapons[i].transform.SetParent(weaponHolster);
        }
    }

    void Equip(int weaponIndex)
    {
        DeactivateWeapons();
        activeWeapon = weapons[weaponIndex];
        activeWeapon.Equip();
        canFire = true;
        weapons[weaponIndex].gameObject.SetActive(true);

        if (OnWeaponSwitch != null)
            OnWeaponSwitch(activeWeapon);
    }

    void SwitchWeapon(int direction)
    {
        canFire = false;

        currentWeaponIndex += direction;

        if (currentWeaponIndex > weapons.Length - 1)
        {
            currentWeaponIndex = 0;
        }

        if (currentWeaponIndex < 0)
            currentWeaponIndex = weapons.Length - 1;

        GameManager.Instance.Timer.Add(() => {
            Equip(currentWeaponIndex);
        }, weaponSwitchTime);
        

    }
}
