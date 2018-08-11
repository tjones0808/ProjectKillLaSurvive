using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    [SerializeField] float rateOfFire;
    [SerializeField] Transform projectile;
    [SerializeField] Transform hand;
    [SerializeField] AudioController audioReload;
    [SerializeField] AudioController audioFire;


    public Transform AimTarget;
    public Vector3 AimTargetOffset;


    public WeaponReloader reloader;

    private ParticleSystem muzzleFireSystem;

    private WeaponRecoil m_WeaponRecoil;
     WeaponRecoil WeaponRecoil
    {
        get
        {
            if (m_WeaponRecoil == null)
                m_WeaponRecoil = GetComponent<WeaponRecoil>();

            return m_WeaponRecoil;
        }
    }

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

        bool isLocalPlayerControlled = AimTarget == null;

    
        if(!isLocalPlayerControlled)
            muzzle.LookAt(AimTarget.position + AimTargetOffset);

        Transform newBullet = Instantiate(projectile, muzzle.position, muzzle.rotation);

        if (isLocalPlayerControlled)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(.5f, .5f, 0));
            RaycastHit hit;
            Vector3 targetPosition = ray.GetPoint(10);

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
            }

            newBullet.transform.LookAt(targetPosition + AimTargetOffset);
        }

        if (this.WeaponRecoil)
            this.WeaponRecoil.Activate();

        FireEffect();
        audioFire.Play();
        canFire = true;
    }
}
