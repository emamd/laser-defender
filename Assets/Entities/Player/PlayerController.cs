using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	public float padding = 1.0f;
	public GameObject projectile;
	public float projectileSpeed;
	public float fireRate;
	public float health;
	public AudioClip fire;
	public AudioClip destroyed;

	float xmin;
	float xmax;

	void Start() {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint (new Vector3(0,0,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3(1,0,distance));
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
	}

	void Fire() {
		Vector3 offset = new Vector3 (0, 1, 0);
		GameObject beam = Instantiate (projectile, transform.position + offset, Quaternion.identity) as GameObject;
		beam.rigidbody2D.velocity = new Vector3(0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint(fire, gameObject.transform.position, 0.8f);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.RightArrow)) {
 			transform.position += Vector3.right * moveSpeed * Time.deltaTime;
		} else if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += Vector3.left * moveSpeed * Time.deltaTime;
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			InvokeRepeating("Fire", 0.00001f, fireRate);
		} else if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke("Fire");
		}

		// restrict player to the game space. 
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
		transform.position = new Vector3 (newX, transform.position.y, transform.position.z);
	}

	void OnTriggerEnter2D(Collider2D col) {
		Projectile projectile = col.gameObject.GetComponent<Projectile> ();
		if (projectile) {
			Debug.Log ("Player hit");

			health -= projectile.GetDamage();
			
			if (health <= 0.0) {
				Debug.Log (health);
				AudioSource.PlayClipAtPoint(destroyed, gameObject.transform.position, 0.8f);
				Destroy(gameObject);
			}
			
			projectile.Hit();
			
		}
	}
}
