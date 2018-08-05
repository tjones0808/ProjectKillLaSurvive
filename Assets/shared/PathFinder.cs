using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PathFinder : MonoBehaviour {

    [HideInInspector]
    public NavMeshAgent Agent;

    [SerializeField] float distanceRemainingThreshold;

    bool m_destinationReached;
    bool destinationReached{
        get
        {
            return m_destinationReached;
        }

        set
        {
            m_destinationReached = value;
            if (m_destinationReached != false)
                OnDestinationReached();
        }
    }

    public event System.Action OnDestinationReached;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();

    }

    public void SetTarget(Vector3 target)
    {
        Agent.SetDestination(target);
        destinationReached = false;
    }

    private void Update()
    {
        if (destinationReached || !Agent.hasPath)
            return;

        if (Agent.remainingDistance < distanceRemainingThreshold)
            destinationReached = true;
    }
}
