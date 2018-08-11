using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(EnemyState))]
public class EnemyPlayer : MonoBehaviour {

    [SerializeField]
    public Scanner playerScanner;

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

    private EnemyState m_enemyState;
    public EnemyState EnemyState
    {
        get
        {
            if (m_enemyState == null)
            {
                m_enemyState = GetComponent<EnemyState>();
            }

            return m_enemyState;
        }
    }

    private void Start()
    {
        pathFinder = GetComponent<PathFinder>();
        pathFinder.Agent.speed = settings.WalkSpeed;
        playerScanner.OnScanReady += Scanner_OnScanReady;
        Scanner_OnScanReady();

        EnemyHealth.OnDeath += EnemyHealth_OnDeath;
        EnemyState.OnModeChanged += EnemyState_OnModeChanged;
    }

    private void EnemyState_OnModeChanged(EnemyState.EMode state)
    {
        pathFinder.Agent.speed = settings.WalkSpeed;

        if (state == EnemyState.EMode.AWARE)
            pathFinder.Agent.speed = settings.RunSpeed;
    }

    void CheckEaseWeapon()
    {
        // check if we can stop aiming
        if (priorityTarget != null)
            return;

        this.EnemyState.CurrentMode = EnemyState.EMode.UNAWARE;
    }

    void CheckContinuePatrol()
    {
        if (priorityTarget != null)
            return;

        pathFinder.Agent.Resume();
    }

    internal void ClearTargetAndScan()
    {
        priorityTarget = null;

        GameManager.Instance.Timer.Add(CheckEaseWeapon, UnityEngine.Random.Range(3,6));
        GameManager.Instance.Timer.Add(CheckContinuePatrol, UnityEngine.Random.Range(10,15));

        Scanner_OnScanReady();
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
            {
                this.EnemyState.CurrentMode = EnemyState.EMode.AWARE;
                OnTargetSelected(priorityTarget);
            }
        }
        
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

    private void Update()
    {        
        if (priorityTarget == null || !EnemyHealth.IsAlive)
            return;

        transform.LookAt(priorityTarget.transform.transform.position);
        
    }

}
