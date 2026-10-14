using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "WavesConfig", menuName = "Scriptable Objects/WavesConfig")]
public class WavesConfig : ScriptableObject
{
    public List<WaveConfig> Waves;
}

[Serializable]
public class WaveConfig
{
    [Min(0)]
    public int EnemyCount;
    [Min(0f)]
    public float SpawnInterval;
    [Min(0f)]
    public float DelayBeforeNextWave;
    public List<Enemy> EnemyPrefabs;

    public bool HasEnemyPrefabs => EnemyPrefabs != null && EnemyPrefabs.Count > 0 && !EnemyPrefabs.Contains(null);

    public Enemy GetRandomEnemyPrefab() => EnemyPrefabs[Random.Range(0, EnemyPrefabs.Count)];
}