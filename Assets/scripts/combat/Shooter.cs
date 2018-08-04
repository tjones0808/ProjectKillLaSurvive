using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    [SerializeField] float rateOfFire;
    [SerializeField] Transform projectile;
    [SerializeField] Transform hand;
    [SerializeField] AudioController audioReload;
    [SerializeField] AudioController audioFire;
    [SerializeField] Transform aimTarget;

    public WeaponReloader reloader;

    private ParticleSystem muzzleFireSystem;



    public void Reload()
    {        
        if (reloader == null)
            return;
        
        reloader.Reload();
        audioReload.Play();
    }

    public float nextFireAllowed;
    public bool canFire;
    Transform muzzle;

    public void Equip()
    {
        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }    

    private void Awake()
    {
        muzzle = transform.Find("Model/Muzzle");
        reloader = GetComponent<WeaponReloader>();
        muzzleFireSystem = muzzle.GetComponent<ParticleSystem>();
    }

    void FireEffect()
    {
        if (muzzleFireSystem == null)
            return;

        muzzleFireSystem.Play();
    }

    public virtual void Fire()
    {
        
        canFire = false;

        if (Time.time < nextFireAllowed)
            return;

        if (reloader != null)
        {
            if (reloader.IsReloading)
                return;
            if (reloader.RoundsRemainingInClip == 0)
                return;

            // need to setup fire modes
            reloader.TakeFromClip(1);
         
        }

        nextFireAllowed = Time.time + rateOfFire;

        muzzle.LookAt(aimTarget);
        

        Instantiate(projectile, muzzle.position, muzzle.rotation);
        FireEffect();
        audioFire.Play();
        canFire = true;
    }
}
