using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
	public GameObject enemyPrefab;
	public float width = 10.0f;
	public float height = 5.0f;
	public float moveSpeed = 7.0f; 

	private bool movingRight = false;
	private float xmin;
	private float xmax;

	// Use this for initialization
	void Start () {
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3(1,0,distance));
		xmin = leftmost.x;
		xmax = rightmost.x;

		foreach (Transform child in transform) { 
			GameObject enemy = Instantiate (enemyPrefab, child.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child.transform; 	
		}
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube (transform.position, new Vector3 (width, height, 0));
	}

	// Update is called once per frame
	void Update () {
		if (movingRight) {
			transform.position += Vector3.right * moveSpeed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * moveSpeed * Time.deltaTime;
		}

		float formationRightEdge = transform.position.x + width / 2;
		float formationLeftEdge = transform.position.x - width / 2;

		if (formationLeftEdge < xmin) {
			movingRight = true;
		} else if (formationRightEdge > xmax) {
			movingRight = false;
		}
	}
}
