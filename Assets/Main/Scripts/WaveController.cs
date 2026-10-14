using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [SerializeField] private WavesConfig _wavesConfig;
    [SerializeField] private EnemySpawner _spawner;

    private int _aliveCount;
    private AwaitableCompletionSource _waveCleared;

    public event Action<int, int> WaveStarted;
    public event Action<int> AliveCountChanged;
    public event Action AllWavesCompleted;

    private async void Start()
    {
        try
        {
            await RunWaves(destroyCancellationToken);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private async Awaitable RunWaves(CancellationToken token)
    {
        IReadOnlyList<WaveConfig> waves = _wavesConfig.Waves;

        for (int i = 0; i < waves.Count; i++)
        {
            WaveConfig wave = waves[i];

            if (!wave.HasEnemyPrefabs)
            {
                Debug.LogWarning($"Wave {i + 1} has no enemy prefabs and was skipped.", _wavesConfig);
                continue;
            }

            WaveStarted?.Invoke(i + 1, waves.Count);

            await SpawnWave(wave, token);
            await WaitForWaveCleared(token);

            bool isLastWave = i == waves.Count - 1;
            if (!isLastWave)
                await Awaitable.WaitForSecondsAsync(wave.DelayBeforeNextWave, token);
        }

        AllWavesCompleted?.Invoke();
    }

    private async Awaitable SpawnWave(WaveConfig wave, CancellationToken token)
    {
        for (int i = 0; i < wave.EnemyCount; i++)
        {
            if (i > 0)
                await Awaitable.WaitForSecondsAsync(wave.SpawnInterval, token);

            Enemy enemy = _spawner.Spawn(wave.GetRandomEnemyPrefab());
            enemy.Died += OnEnemyDied;
            SetAliveCount(_aliveCount + 1);
        }
    }

    private async Awaitable WaitForWaveCleared(CancellationToken token)
    {
        if (_aliveCount == 0)
            return;

        var waveCleared = new AwaitableCompletionSource();
        _waveCleared = waveCleared;

        using (token.Register(() => waveCleared.TrySetCanceled()))
            await waveCleared.Awaitable;

        _waveCleared = null;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        enemy.Died -= OnEnemyDied;
        SetAliveCount(_aliveCount - 1);

        if (_aliveCount == 0)
            _waveCleared?.TrySetResult();
    }

    private void SetAliveCount(int count)
    {
        _aliveCount = count;
        AliveCountChanged?.Invoke(_aliveCount);
    }
}