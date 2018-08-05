using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathFinder))]
[RequireComponent(typeof(EnemyPlayer))]
public class EnemyPatrol : MonoBehaviour {

    [SerializeField]
    WaypointController waypointController;

    [SerializeField]
    float waitTimeMin;
    [SerializeField]
    float waitTimeMax;

    PathFinder pathFinder;
    EnemyPlayer m_enemyPlayer;
    public EnemyPlayer EnemyPlayer
    {
        get
        {
            if (m_enemyPlayer == null)
                m_enemyPlayer = GetComponent<EnemyPlayer>();

            return m_enemyPlayer;
        }
    }

    private void Start()
    {
        waypointController.SetNextWaypoint();
    }

    private void Awake()
    {
        pathFinder = GetComponent<PathFinder>();
        pathFinder.OnDestinationReached += PathFinder_OnDestinationReached;
        waypointController.OnWaypointChanged += WaypointController_OnWaypointChanged;

        EnemyPlayer.EnemyHealth.OnDeath += EnemyHealth_OnDeath;
    }

    private void EnemyHealth_OnDeath()
    {
        pathFinder.Agent.Stop();
    }

    private void WaypointController_OnWaypointChanged(Waypoint waypoint)
    {
        pathFinder.SetTarget(waypoint.transform.position);
    }
    private void PathFinder_OnDestinationReached()
    {
        // assume we are patrolling
        GameManager.Instance.Timer.Add(waypointController.SetNextWaypoint, Random.Range(waitTimeMin, waitTimeMax));
    }
}
