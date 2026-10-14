using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Scriptable Objects/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Min(0f)]
    public float Speed;
    public Vector2 MoveDirection = Vector2.down;
    public Vector2 Direction => MoveDirection == Vector2.zero ? Vector2.down : MoveDirection.normalized;
}