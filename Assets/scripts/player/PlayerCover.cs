using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCover : MonoBehaviour {

    private bool canTakeCover;
    private bool isInCover;
    private RaycastHit closetHit;

    [SerializeField]
    int numberOfRays;

    [SerializeField]
    LayerMask coverMask;

    bool isAiming
    {
        get
        {
            if(GameManager.Instance.LocalPlayer != null)
                return GameManager.Instance.LocalPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMEDFIRING ||
                GameManager.Instance.LocalPlayer.PlayerState.WeaponState == PlayerState.EWeaponState.AIMING;

            return false;
        }
    }

    
    internal void SetPlayerCoverAllowed(bool value)
    {
        canTakeCover = value;

        if (!canTakeCover && isInCover)
            ExecuteCoverToggle();
    }

    private void Update()
    {
        if (isAiming && isInCover)
        {
            ExecuteCoverToggle();
            return;
        }

        if (!canTakeCover)
            return;

        if (GameManager.Instance.InputController.CoverToggle)
        {
            TakeCover();
        }
    }

    void ExecuteCoverToggle()
    {
        isInCover = !isInCover;

        GameManager.Instance.EventBus.RaiseEvent("CoverToggle");

        transform.rotation = Quaternion.LookRotation(closetHit.normal) * Quaternion.Euler(0, 180f, 0);
    }

    void TakeCover()
    {
        FindCoverAroundPlayer();

        if (closetHit.distance == 0)
            return;

        ExecuteCoverToggle();
    }

    private void FindCoverAroundPlayer()
    {
        closetHit = new RaycastHit();
        float angleStep = 360 / numberOfRays;
        for (int i = 0; i < numberOfRays; i++)
        {
            Quaternion angle = Quaternion.AngleAxis(i * angleStep, transform.up);

            CheckClosestPoint(angle);
        }
        Debug.DrawLine(transform.position + Vector3.up * .3f, closetHit.point, Color.magenta, .5f);
    }

    private void CheckClosestPoint(Quaternion angle)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * .3f, angle * Vector3.forward, out hit, 5f, coverMask))
        {
            if (closetHit.distance == 0 || hit.distance < closetHit.distance)
                closetHit = hit;
        }
    }
}
