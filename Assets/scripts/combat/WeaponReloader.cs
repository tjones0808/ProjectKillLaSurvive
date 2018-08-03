using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloader : MonoBehaviour
{

    [SerializeField] int maxAmmo;
    [SerializeField] float reloadTime;
    [SerializeField] int clipSize;
    [SerializeField] Container inventory;

    public int shotsFiredInClip;
    Guid containerItemId;
    bool isReloading;

    public event System.Action OnAmmoChanged;

    public int RoundsRemainingInClip
    {
        get
        {
            return clipSize - shotsFiredInClip;
        }
    }

    public int RoundsRemainingInIventory
    {
        get
        {
            return inventory.GetAmountRemaining(containerItemId);
        }
    }

    public bool IsReloading
    {
        get
        {
            return isReloading;
        }
    }

    private void Awake()
    {

        containerItemId = inventory.Add(this.name, maxAmmo);


    }
    public void Reload()
    {
        if (isReloading)
            return;

        isReloading = true;
        int amountFromInventory = inventory.TakeFromContainer(containerItemId, clipSize - RoundsRemainingInClip);

        if (amountFromInventory > 0)
            print("Cock locked and ready to rock.");
        else
            print("inventory empty");

        GameManager.Instance.Timer.Add(() =>
        {
            ExecuteReload(amountFromInventory);
        }, reloadTime);

    }

    private void ExecuteReload(int amount)
    {
        isReloading = false;
        shotsFiredInClip -= amount;

        if (OnAmmoChanged != null)
            OnAmmoChanged();

    }

    public void TakeFromClip(int amount)
    {
        shotsFiredInClip += amount;

        if (OnAmmoChanged != null)
            OnAmmoChanged();
    }
}
