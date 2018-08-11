using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour {

    [SerializeField]
    Destructable[] targets;

    int targetsDestroyedCount;

    private void Start()
    {
        targetsDestroyedCount = 0;
        foreach(var target in targets)
            target.OnDeath += Target_OnDeath;

    }

    private void Target_OnDeath()
    {
        targetsDestroyedCount++;

        if (targetsDestroyedCount == targets.Length)
        {
            print("all destroyed: targets killed = " + targetsDestroyedCount + " / " + targets.Length);
            GameManager.Instance.EventBus.RaiseEvent("AllEnemiesKilled");
        }
        
    }
}
