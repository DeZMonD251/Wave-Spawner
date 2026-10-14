using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private PlayArea _playArea;
    [SerializeField, Min(1)] private int _poolCapacity = 10;
    [SerializeField, Min(1)] private int _poolMaxSize = 50;

    private readonly Dictionary<Enemy, ObjectPool<Enemy>> _poolByPrefab = new();
    private readonly Dictionary<Enemy, ObjectPool<Enemy>> _poolByInstance = new();

    public Enemy Spawn(Enemy prefab)
    {
        if (!_poolByPrefab.TryGetValue(prefab, out ObjectPool<Enemy> pool))
        {
            pool = CreatePool(prefab);
            _poolByPrefab.Add(prefab, pool);
        }

        return pool.Get();
    }

    private ObjectPool<Enemy> CreatePool(Enemy prefab)
    {
        ObjectPool<Enemy> pool = null;
        pool = new ObjectPool<Enemy>(
            () => Create(prefab, pool),
            OnGet,
            OnRelease,
            OnDestroyPooled,
            collectionCheck: true,
            _poolCapacity,
            _poolMaxSize);

        return pool;
    }

    private Enemy Create(Enemy prefab, ObjectPool<Enemy> pool)
    {
        Enemy enemy = Instantiate(prefab, _playArea.GetRandomSpawnPoint(), Quaternion.identity, transform);
        _poolByInstance.Add(enemy, pool);
        return enemy;
    }

    private void OnGet(Enemy enemy)
    {
        enemy.transform.position = _playArea.GetRandomSpawnPoint();
        enemy.ResetState();
        enemy.Died += Release;
        enemy.gameObject.SetActive(true);
    }

    private void Release(Enemy enemy) => _poolByInstance[enemy].Release(enemy);

    private void OnRelease(Enemy enemy)
    {
        enemy.Died -= Release;
        enemy.gameObject.SetActive(false);
    }

    private void OnDestroyPooled(Enemy enemy)
    {
        _poolByInstance.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}