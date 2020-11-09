using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts;
using JetBrains.Annotations;
using UnityEngine;

public class TargetSelector : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private EnemyController _selectedTarget = null;
    [SerializeField] private EnemyController _enemyCurrentlyInCombat = null;
    [SerializeField] public PlayerConfig _playerConfig;
    [SerializeField] public GameConfig _gameConfig;
#pragma warning restore 649
    public EnemyController SelectedTarget
    {
        get => _selectedTarget;
        private set
        {
            if (_selectedTarget != null) _selectedTarget.IsHighlighted = false;
            _selectedTarget = value;
            if (_selectedTarget != null) _selectedTarget.IsHighlighted = true;
        }
    }

    public EnemyController EnemyCurrentlyInCombat
    {
        get => _enemyCurrentlyInCombat;
        set => _enemyCurrentlyInCombat = value;
    }

    // Update is called once per frame
    public void SelectTarget(Transform target)
    {
        if (target == null) return;
        
        SelectedTarget = _gameManager.Enemies[target];
    }
    
    public void SelectNearestTarget(Vector3 position)
    {
        var collidersInRange = Physics.OverlapSphere(position, _playerConfig.AttackRange, _gameConfig.EnemyLayers);
        if (collidersInRange.Length == 0)
        {
            SelectedTarget = null;
            return;
        }
        
        var closestEnemy = collidersInRange
            .OrderBy(c => (c.transform.position - _gameManager.PlayerPosition).sqrMagnitude)
            .FirstOrDefault();
        SelectTarget(closestEnemy.transform.root);
    }
}
