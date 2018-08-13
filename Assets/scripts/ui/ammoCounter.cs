using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ammoCounter : MonoBehaviour {

    [SerializeField] Text text;
    
    PlayerShoot playerShoot;
    WeaponReloader reloader;
	// Use this for initialization
	void Awake () {
        GameManager.Instance.OnLocalPlayerJoined += Instance_OnLocalPlayerJoined;
	}

    private void Instance_OnLocalPlayerJoined(Player obj)
    {
        // assuming player obj has container is bad code
        playerShoot = obj.GetComponent<PlayerShoot>();

        playerShoot.OnWeaponSwitch += PlayerShoot_OnWeaponSwitch;       
        
    }

    private void PlayerShoot_OnWeaponSwitch(Shooter activeWeapon)
    {
        reloader = activeWeapon.Reloader;
        reloader.OnAmmoChanged += HandleOnAmmoChanged;
        HandleOnAmmoChanged();
    }
    

    private void HandleOnAmmoChanged()
    {
        int amountInInventory = reloader.RoundsRemainingInIventory;
        int amountInClip = reloader.RoundsRemainingInClip;
        text.text = string.Format("{0}/{1}", amountInClip, amountInInventory);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
