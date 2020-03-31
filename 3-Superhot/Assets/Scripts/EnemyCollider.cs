using UnityEngine;

public class EnemyCollider: MonoBehaviour {
    [SerializeField] private TriangleExplosion _explosion;
    
    public void Destroy(Vector3 position) {
        _explosion.Destroy(position);
    }
}