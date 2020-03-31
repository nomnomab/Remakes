using System;
using UnityEngine;

public class Bullet: MonoBehaviour {
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ignore")) {
            return;
        }

        if (other.transform.tag.Equals("Enemy")) {
            other.gameObject.GetComponent<EnemyCollider>().Destroy(transform.position);
        }
        
        Destroy(gameObject);
    }
}