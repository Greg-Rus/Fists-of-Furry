using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UserInput : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera; 
    [SerializeField] private NavMeshAgent _navigation; 
    [SerializeField] private GameManager _gameManager; 
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
        {
            _navigation.SetDestination(hit.point);
        }
    }
}