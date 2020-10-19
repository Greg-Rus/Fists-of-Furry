using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts;
using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    public GameManager _gameManager;
    public EnemyController _selectedTarget = null;
    public PlayerConfig _playerConfig;
    public GameConfig _gameConfig;

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

    // Update is called once per frame
    public void SelectTarget(Transform target)
    {
        if (target == null) return;
        
        var sqrDistanceToPlayer = (target.position - _gameManager.PlayerPosition).sqrMagnitude;
        if(sqrDistanceToPlayer > _playerConfig.AttackRange.Sqrd());
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
