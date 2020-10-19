using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class UserInput : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera; 
    [SerializeField] private NavMeshAgent _navigation; 
    [SerializeField] private TargetSelector _targetSelector; 
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private GameConfig _gameConfig;
    public Vector3 InputDestination { get; private set; }

    // Update is called once per frame
    void Update()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit, 1000f, _gameConfig.EnemyLayers))
        {
            _targetSelector.SelectTarget(hit.transform.root);
        }
        else if (Physics.Raycast(ray, out hit, 1000f, _gameConfig.GroundLayers))
        {
            _targetSelector.SelectNearestTarget(hit.point);
        }
        
        InputDestination = hit.point;
    }
}