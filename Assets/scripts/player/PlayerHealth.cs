using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Destructable {

    [SerializeField]
    Ragdoll ragDoll;

    [SerializeField] SpawnPoint[] spawnPoints;
    void SpawnAtNewSpawnPoint()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        transform.position = spawnPoints[spawnIndex].transform.position;
        transform.rotation = spawnPoints[spawnIndex].transform.rotation;
    }

    public override void Die()
    {
        base.Die();
        ragDoll.EnableRagdoll(true);
        GameManager.Instance.Timer.Add(SpawnAtNewSpawnPoint, 5);
    }

    [ContextMenu("Test Die")]
    void TestDie()
    {
        Die();
    }
}
