using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : PickupItem {

    [SerializeField] EWeaponType weaponType;
    [SerializeField] float respawnTime;
    [SerializeField] int amount;
    public override void OnPickup(Transform item)
    {
        // not safe
        var playerInventory = item.GetComponentInChildren<Container>();

        GameManager.Instance.Respawner.Despawn(gameObject, respawnTime);

        playerInventory.Put(weaponType.ToString(), amount);


        // needs refactoring, too many dots need to check for reloader and nulls
        item.GetComponent<Player>().playerShoot.ActiveWeapon.reloader.HandleOnAmmoChanged();

    }
}
