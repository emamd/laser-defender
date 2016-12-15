using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	public float health = 150f;
	public GameObject enemyProjectile;
	public float enemyProjectileSpeed;
	public float shotsPerSecond;
	public int hitValue = 25;
	public int killValue = 100;

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
		Vector3 startPosition = transform.position + new Vector3(0, -1, 0);

		GameObject beam = Instantiate (enemyProjectile, startPosition, Quaternion.identity) as GameObject;
		beam.rigidbody2D.velocity = new Vector2 (0, -enemyProjectileSpeed);
		
	}

	void OnTriggerEnter2D(Collider2D col) {
		Projectile projectile = col.gameObject.GetComponent<Projectile> ();
		if (projectile) {
			health -= projectile.GetDamage();

			if (health <= 0.0) {
				Debug.Log (health);
				scoreKeeper.Score(killValue);
				Destroy(gameObject);
			} else {
				scoreKeeper.Score(hitValue);
			}

			projectile.Hit();

		}
	}


}
