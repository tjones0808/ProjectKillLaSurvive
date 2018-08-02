using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    [SerializeField] float rateOfFire;
    [SerializeField] Transform projectile;

    [HideInInspector]
    public Transform muzzle;

    private WeaponReloader reloader;
    public void Reload()
    {
        print("step 6");
        if (reloader == null)
            return;

        print("step 3");
        reloader.Reload();

    }

    public float nextFireAllowed;
    public bool canFire;

    private void Awake()
    {
        muzzle = transform.Find("Muzzle");
        reloader = GetComponent<WeaponReloader>();
    }

    public virtual void Fire()
    {
        
        canFire = false;

        if (Time.time < nextFireAllowed)
            return;

        if (reloader != null)
        {
            print("step 1");
            if (reloader.IsReloading)
                return;
            if (reloader.RoundsRemainingInClip == 0)
                return;

            // need to setup fire modes
            reloader.TakeFromClip(1);
            print("step 2");
        }

        nextFireAllowed = Time.time + rateOfFire;

        Instantiate(projectile, muzzle.position, muzzle.rotation);
        
        canFire = true;
    }
}
