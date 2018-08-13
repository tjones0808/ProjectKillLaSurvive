using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    [SerializeField] float weaponSwitchTime;

    Shooter[] weapons;

    int currentWeaponIndex;

    //public but not a setting
    [HideInInspector]
    public bool CanFire;

    Transform weaponHolster;

    public event System.Action<Shooter> OnWeaponSwitch;

    Shooter m_activeWeapon;
    public Shooter ActiveWeapon
    {
        get
        {
            return m_activeWeapon;
        }
    }


    private void Awake()
    {
        weaponHolster = transform.FindChild("Weapons");
        weapons = transform.FindChild("Weapons").GetComponentsInChildren<Shooter>();

        if (weapons.Length > 0)
            Equip(0);

        CanFire = true;

    }

    public Vector3 GetImpactPoint()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        RaycastHit hit;
        Vector3 targetPosition = ray.GetPoint(10);

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return transform.position + transform.forward * 200;

    }

    void DeactivateWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
            weapons[i].transform.SetParent(weaponHolster);
        }
    }

    internal void Equip(int weaponIndex)
    {
        DeactivateWeapons();
        CanFire = true;

        m_activeWeapon = weapons[weaponIndex];
        m_activeWeapon.Equip();
        weapons[weaponIndex].gameObject.SetActive(true);

        if (OnWeaponSwitch != null)
            OnWeaponSwitch(ActiveWeapon);
    }

    internal void SwitchWeapon(int direction)
    {
        CanFire = false;

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
