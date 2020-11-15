using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private PrefabConfig _prfabConfig;
    [SerializeField] private EnemyConfig _enemyCofnig;
    [SerializeField] private PlayerController _playerController;
#pragma warning restore 649

    private Dictionary<Transform, EnemyController> _enemies;

    private float _enemySpawnTimer = 0f;

    public Dictionary<Transform, EnemyController> Enemies => _enemies;
    public Vector3 PlayerPosition => _playerController.transform.position;

    // Start is called before the first frame update
    void OnEnable()
    {
        _enemies = new Dictionary<Transform, EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemySpawnTimer();
    }

    private void UpdateEnemySpawnTimer()
    {
        if (_enemySpawnTimer <= 0)
        {
            SpawnEnemy();
            _enemySpawnTimer = _gameConfig.EnemySpawnSpeed;
        }
        else
        {
            _enemySpawnTimer -= Time.smoothDeltaTime;
        }
    }

    private void SpawnEnemy()
    {
        var enemy = Instantiate(_prfabConfig.EnemyPrefab, GetEnemySpawnPosition(), Quaternion.identity);
        var enemyController = enemy.GetComponent<EnemyController>();
        enemyController.Setup(_enemyCofnig, _playerController, GetHitOrder(GetEnemyHp()));
        Enemies.Add(enemy.transform, enemyController);
    }

    private Vector3 GetEnemySpawnPosition()
    {
        var randomInCircle = UnityEngine.Random.insideUnitCircle.normalized;
        var offset = new Vector3(randomInCircle.x, 0f, randomInCircle.y) * _gameConfig.EnemySpawnRadius;
        return _playerController.transform.position + offset;
    }

    private int GetEnemyHp()
    {
        var roll = UnityEngine.Random.value;
        var hpMapping = _enemyCofnig.HpProbabilityMappings.First(map => roll <= map.Range);
        return hpMapping.Hp;
    }

    private HitTypes[] GetHitOrder(int hp)
    {
        var hitOrder = new HitTypes[hp];
        for (int i = 0; i < hp; i++)
        {
            hitOrder[i] = GetRandomHitType();
        }

        return hitOrder;
    }

    private HitTypes GetRandomHitType()
    {
        var roll = UnityEngine.Random.Range(0, 3);
        return (HitTypes) roll;
    }
}

public enum HitTypes
{
    Any = 0,
    Punch = 1,
    Kick = 2
}
