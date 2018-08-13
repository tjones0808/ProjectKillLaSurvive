using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    [SerializeField] float rateOfFire;
    [SerializeField] Transform projectile;
    [SerializeField] Transform hand;
    [SerializeField] AudioController audioReload;
    [SerializeField] AudioController audioFire;

    Player player;

    public Vector3 AimPoint;
    public Vector3 AimTargetOffset;


    public WeaponReloader Reloader;

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

    public void SetAimPoint(Vector3 target)
    {
        AimPoint = target;
    }

    public void Reload()
    {
        if (Reloader == null)
            return;

        if (player.IsLocalPlayer)
        {
            Reloader.Reload();
            audioReload.Play();
        }

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

        player = GetComponentInParent<Player>();

        Reloader = GetComponent<WeaponReloader>();
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

        if (player.IsLocalPlayer && Reloader != null)
        {
            if (Reloader.IsReloading)
                return;
            if (Reloader.RoundsRemainingInClip == 0)
                return;

            // need to setup fire modes
            Reloader.TakeFromClip(1);
        }

        nextFireAllowed = Time.time + rateOfFire;

        muzzle.LookAt(AimPoint + AimTargetOffset);

        Transform newBullet = Instantiate(projectile, muzzle.position, muzzle.rotation);        

        if (this.WeaponRecoil)
            this.WeaponRecoil.Activate();

        FireEffect();
        audioFire.Play();
        canFire = true;
    }
}
