using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private EnemyConfig _config;

    [SerializeField] private Rigidbody2D _body;

    private void FixedUpdate()
    {
        Vector2 step = _config.Direction * (_config.Speed * Time.fixedDeltaTime);
        _body.MovePosition(_body.position + step);
    }
}