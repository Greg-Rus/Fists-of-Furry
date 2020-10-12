using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private PrefabConfig _prfabConfig;
    [SerializeField] private EnemyConfig _enemyCofnig;
    [SerializeField] private GameObject _player;

    private float _enemySpawnTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
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
        enemy.GetComponent<EnemyController>().Setup(_enemyCofnig, _player.transform);
    }

    private Vector3 GetEnemySpawnPosition()
    {
        var randomInCircle = UnityEngine.Random.insideUnitCircle.normalized;
        var offset = new Vector3(randomInCircle.x, 0f, randomInCircle.y) * _gameConfig.EnemySpawnRadius;
        return _player.transform.position + offset;
    }
}
