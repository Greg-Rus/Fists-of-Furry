using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class UserInput : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private Camera _mainCamera; 
    [SerializeField] private TargetSelector _targetSelector; 
    [SerializeField] private PlayerConfig _playerConfig;
    [SerializeField] private GameConfig _gameConfig;
    private Vector3 _lastMousePosition = Vector3.zero;
    
#pragma warning restore 649
    public Vector3 InputDestination { get; private set; }
    public AttackType LastAttackInput;

    // Update is called once per frame
    void LateUpdate()
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        var mouseMoved = (_lastMousePosition - Input.mousePosition).sqrMagnitude >= _gameConfig.MinimumMousePositionDelta;
        _lastMousePosition = Input.mousePosition;
        
        if(Physics.Raycast(ray, out hit, 1000f, _gameConfig.EnemyLayers))
        {
            if(mouseMoved) _targetSelector.SelectTarget(hit.transform.root);
        }
        else if (Physics.Raycast(ray, out hit, 1000f, _gameConfig.GroundLayers))
        {
            if(mouseMoved) _targetSelector.SelectNearestTarget(hit.point);
        }

        InputDestination = hit.point;

        if (Input.GetMouseButtonUp(0))
        {
            LastAttackInput = AttackType.Punch;
        }

        if (Input.GetMouseButtonUp(1))
        {
            LastAttackInput = AttackType.Kick;
        }
    }
}