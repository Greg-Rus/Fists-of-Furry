using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterNavigation : MonoBehaviour
{
    private NavMeshAgent _agent;

    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 desiredPosition)
    {
        _agent.SetDestination(desiredPosition);
    }

    public void Setup(float speed)
    {
        _agent.speed = speed;
    }
}