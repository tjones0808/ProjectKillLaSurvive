using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : PickupItem {

    [SerializeField] EWeaponType weaponType;
    [SerializeField] float respawnTime;
    [SerializeField] int amount;

    private void Start()
    {
        GameManager.Instance.EventBus.AddListener("EnemyDeath", new EventBus.EventListener()
        {
            Method = OnEnemyDeath
        });
    }

    private void OnEnemyDeath()
    {
        //print("Enemy Death Listener" + transform.name);

    }

    public override void OnPickup(Transform item)
    {
        // not safe
        var playerInventory = item.GetComponentInChildren<Container>();

        GameManager.Instance.Respawner.Despawn(gameObject, respawnTime);

        playerInventory.Put(weaponType.ToString(), amount);


        // needs refactoring, too many dots need to check for reloader and nulls
        item.GetComponent<Player>().PlayerShoot.ActiveWeapon.Reloader.HandleOnAmmoChanged();

    }
}
