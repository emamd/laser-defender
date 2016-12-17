using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	public float health = 150f;
	public GameObject enemyProjectile;
	public float enemyProjectileSpeed;
	public float shotsPerSecond;
	public int hitValue = 25;
	public int killValue = 100;
	public AudioClip fire;
	public AudioClip destroyed;

	private ScoreKeeper scoreKeeper;

	void Start(){
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper> ();
	}

	void Update() {
		float probability = Time.deltaTime * shotsPerSecond;
		if (Random.value < probability) {
			Fire ();
		}
	}

	void Fire() {
		GameObject beam = Instantiate (enemyProjectile, transform.position, Quaternion.identity) as GameObject;
		beam.rigidbody2D.velocity = new Vector2 (0, -enemyProjectileSpeed);
		AudioSource.PlayClipAtPoint(fire, gameObject.transform.position, 0.8f);
	}

	void OnTriggerEnter2D(Collider2D col) {
		Projectile projectile = col.gameObject.GetComponent<Projectile> ();
		if (projectile) {
			health -= projectile.GetDamage();

			if (health <= 0.0) {
				Die();
			} else {
				scoreKeeper.Score(hitValue);
			}

			projectile.Hit();

		}
	}

	void Die() {
		scoreKeeper.Score(killValue);
		AudioSource.PlayClipAtPoint(destroyed, gameObject.transform.position, 0.8f);
		Destroy(gameObject);
	}

}
