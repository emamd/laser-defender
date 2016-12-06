using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;
	public float padding = 1.0f;
	public GameObject projectile;
	public float projectileSpeed;
	public float fireRate;
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
		GameObject beam = Instantiate (projectile, transform.position, Quaternion.identity) as GameObject;
		beam.rigidbody2D.velocity = new Vector3(0, projectileSpeed, 0);
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
}
