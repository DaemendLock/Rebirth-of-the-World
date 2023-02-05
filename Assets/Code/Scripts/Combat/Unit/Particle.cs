using Projectiles;
using UnityEngine;

public class Particle : MonoBehaviour {
    public ProjectileID linkedProjectile;

    private void OnTriggerEnter(Collider other) {
        linkedProjectile.Hit(other.gameObject.GetComponent<Unit>());
    }
}