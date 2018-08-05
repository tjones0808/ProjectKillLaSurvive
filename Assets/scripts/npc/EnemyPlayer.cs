using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyPlayer : MonoBehaviour {

    PathFinder pathFinder;
    [SerializeField]
    Scanner playerScanner;
    Player priorityTarget;
    List<Player> myTargets;

    private EnemyHealth m_enemyHealth;
    public EnemyHealth EnemyHealth
    {
        get
        {
            if (m_enemyHealth == null)
            {
                m_enemyHealth = GetComponent<EnemyHealth>();
            }

            return m_enemyHealth;
        }
    }

    private void Start()
    {
        pathFinder = GetComponent<PathFinder>();
        playerScanner.OnScanReady += Scanner_OnScanReady;
        Scanner_OnScanReady();
    }

    private void Scanner_OnScanReady()
    {
        if (priorityTarget != null)
            return;

        myTargets = playerScanner.ScanForTargets<Player>();

        if (myTargets.Count == 1)
            priorityTarget = myTargets[0];
        else
            SelectClosestTarget();

        if (priorityTarget != null)
            SetDestinationToPriorityTarget();
    }
    private void SetDestinationToPriorityTarget()
    {
        pathFinder.SetTarget(priorityTarget.transform.position);
    }

    private void SelectClosestTarget()
    {
        float closestTarget = playerScanner.ScanRange;
        foreach (var possibleTarget in myTargets)
        {
            if (Vector3.Distance(transform.position, possibleTarget.transform.position) < closestTarget)
                priorityTarget = possibleTarget;
        }
    }
    
}
