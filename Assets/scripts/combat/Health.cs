using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Destructable {

    [SerializeField] float inSeconds;
    public override void Die()
    {
        print("We died shithead.");

        base.Die();

        GameManager.Instance.Respawner.Despawn(gameObject, inSeconds);

        
    }

    private void OnEnable()
    {
        Reset();
    }

    public override void TakeDamage(float amount)
    {
        print("Remaining: " + HitPointsRemaining);

        base.TakeDamage(amount);
    }
    
}
