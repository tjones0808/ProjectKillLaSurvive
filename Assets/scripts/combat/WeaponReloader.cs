using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponReloader : MonoBehaviour
{

    [SerializeField] int maxAmmo;
    [SerializeField] float reloadTime;
    [SerializeField] int clipSize;

    int ammo;
    public int shotsFiredInClip;

    bool isReloading;

    public int RoundsRemainingInClip
    {
        get
        {
            return clipSize - shotsFiredInClip;
        }
    }

    public bool IsReloading
    {
        get
        {
            return isReloading;
        }
    }
    public void Reload()
    {
        print("step 5");
        if (isReloading)
            return;

        print("Reloading");
        isReloading = true;

        GameManager.Instance.Timer.Add(ExecuteReload, reloadTime);
    }

    private void ExecuteReload()
    {
        print("Cock locked and ready to rock.");
        isReloading = false;

        ammo -= shotsFiredInClip;

        shotsFiredInClip = 0;

        if (ammo < 0)
        {
            ammo = 0;
            shotsFiredInClip += -ammo;
        }
    }

    public void TakeFromClip(int amount)
    {
        print("step 4");
        shotsFiredInClip += amount;
    }
}
