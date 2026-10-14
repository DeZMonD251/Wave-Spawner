using TMPro;
using UnityEngine;

public class WaveHUD : MonoBehaviour
{
    [SerializeField] private WaveController _waveController;
    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _aliveText;
    [SerializeField] private GameObject _gameOverPanel;

    private void Awake()
    {
        _gameOverPanel.SetActive(false);
        ShowAliveCount(0);
    }

    private void OnEnable()
    {
        _waveController.WaveStarted += ShowWave;
        _waveController.AliveCountChanged += ShowAliveCount;
        _waveController.AllWavesCompleted += ShowGameOver;
    }

    private void OnDisable()
    {
        _waveController.WaveStarted -= ShowWave;
        _waveController.AliveCountChanged -= ShowAliveCount;
        _waveController.AllWavesCompleted -= ShowGameOver;
    }

    private void ShowWave(int number, int total) => _waveText.text = $"Wave: {number}/{total}";

    private void ShowAliveCount(int count) => _aliveText.text = $"Enemies: {count}";

    private void ShowGameOver() => _gameOverPanel.SetActive(true);
}