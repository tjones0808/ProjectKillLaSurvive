using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Destructable {

    [SerializeField]
    Ragdoll ragDoll;

    public override void Die()
    {
        base.Die();
        ragDoll.EnableRagdoll(true);
        GameManager.Instance.EventBus.RaiseEvent("EnemyDeath");
    }
}
