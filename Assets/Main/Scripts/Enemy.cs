using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour, IPointerClickHandler
{
    private bool _isDead;

    public event Action<Enemy> Died;

    public void ResetState() => _isDead = false;

    public void OnPointerClick(PointerEventData eventData) => Kill();

    public void Kill()
    {
        if (_isDead)
            return;

        _isDead = true;
        Died?.Invoke(this);
    }
}