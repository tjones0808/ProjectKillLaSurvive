using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyPlayer : MonoBehaviour {

    [SerializeField]
    Scanner playerScanner;

    [SerializeField]
    SwatSoldier settings;

    PathFinder pathFinder;
    Player priorityTarget;
    List<Player> myTargets;

    public event System.Action<Player> OnTargetSelected;

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
        pathFinder.Agent.speed = settings.WalkSpeed;
        playerScanner.OnScanReady += Scanner_OnScanReady;
        Scanner_OnScanReady();

        EnemyHealth.OnDeath += EnemyHealth_OnDeath;
    }

    private void EnemyHealth_OnDeath()
    {
        
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
        {
            if (OnTargetSelected != null)
                OnTargetSelected(priorityTarget);
        }
        
    }

    //private void SetDestinationToPriorityTarget()
    //{
    //    pathFinder.SetTarget(priorityTarget.transform.position);
    //}

    private void SelectClosestTarget()
    {
        float closestTarget = playerScanner.ScanRange;
        foreach (var possibleTarget in myTargets)
        {
            if (Vector3.Distance(transform.position, possibleTarget.transform.position) < closestTarget)
                priorityTarget = possibleTarget;
        }
    }

    private void Update()
    {
        if (priorityTarget == null)
            return;

        transform.LookAt(priorityTarget.transform.transform.position);
        
    }

}
