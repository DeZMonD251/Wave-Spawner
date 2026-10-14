using UnityEngine;

public class PlayArea : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _spawnInset = 0.5f;

    [SerializeField]
    private BoxCollider2D _area;

    public Vector2 GetRandomSpawnPoint()
    {
        Bounds bounds = _area.bounds;
        float x = Random.Range(bounds.min.x + _spawnInset, bounds.max.x - _spawnInset);
        return new Vector2(x, bounds.max.y - _spawnInset);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Enemy _))
            return;

        Rigidbody2D body = other.attachedRigidbody;
        if (body != null)
            body.position = GetRandomSpawnPoint();
    }
}