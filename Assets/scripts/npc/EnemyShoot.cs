using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Extensions;

[RequireComponent(typeof(EnemyPlayer))]
public class EnemyShoot : WeaponController {

    [SerializeField]
    float shootingSpeed;

    [SerializeField]
    float burstDurationMax;

    [SerializeField]
    float burstDurationMin;

    EnemyPlayer enemyPlayer;

    bool shouldFire;

    Vector3 aimTarget;

    private void Start()
    {
        enemyPlayer = GetComponent<EnemyPlayer>();
        enemyPlayer.OnTargetSelected += EnemyPlayer_OnTargetSelected;
    }

    private void EnemyPlayer_OnTargetSelected(Player obj)
    {
        aimTarget = obj.transform.position;
        ActiveWeapon.SetAimPoint(obj.transform.position);
        ActiveWeapon.AimTargetOffset = Vector3.up * 1.5f;

        StartBurst();

    }

    void CrouchState()
    {
        // 25% chance to take cover 0, 1, 2,3
        bool takeCover = Random.Range(0, 3) == 0;

        if (!takeCover)
            return;

        float distanceToTarget = Vector3.Distance(transform.position, aimTarget);

        if (distanceToTarget > 15)
        {
            enemyPlayer.GetComponent<EnemyAnimation>().IsCrouched = true;
        }
    }

    void StartBurst()
    {
        if (!enemyPlayer.EnemyHealth.IsAlive && !CanSeeTarget())
            return;

        CheckReload();
        CrouchState();
        shouldFire = true;

        GameManager.Instance.Timer.Add(EndBurst, Random.Range(burstDurationMin, burstDurationMax));
    }

    void EndBurst()
    {
        shouldFire = false;
        if (!enemyPlayer.EnemyHealth.IsAlive)
            return;

        CheckReload();
        CrouchState();

        if(CanSeeTarget())
            GameManager.Instance.Timer.Add(StartBurst, shootingSpeed);
    }

    bool CanSeeTarget()
    {
        if (!transform.InLineOfSight(aimTarget, 90, enemyPlayer.playerScanner.mask, Vector3.up))
        {
            enemyPlayer.ClearTargetAndScan();
            return false;
        }
        return true;
    }

    void CheckReload()
    {
        if (ActiveWeapon.Reloader.RoundsRemainingInClip == 0)
        {
            CrouchState();
            ActiveWeapon.Reload();
        }
    }

    private void Update()
    {
        if (!shouldFire || !CanFire || !enemyPlayer.EnemyHealth.IsAlive)
            return;

        ActiveWeapon.Fire();
    }

}
