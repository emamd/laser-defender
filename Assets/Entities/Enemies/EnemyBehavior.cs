using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	public float health = 150f;

	void OnTriggerEnter2D(Collider2D col) {
		Projectile projectile = col.gameObject.GetComponent<Projectile> ();
		if (projectile) {
			health -= projectile.GetDamage();

			if (health <= 0.0) {
				Debug.Log (health);
				Destroy(gameObject);
			}

			projectile.Hit();

		}
	}


}
