using UnityEngine;
using System.Collections;

public class EnemyFormation : MonoBehaviour {
	public GameObject enemyPrefab;
	public float width = 10.0f;
	public float height = 5.0f;
	public float moveSpeed = 7.0f; 
	public float spawnDelay = 0.5f;

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

		SpawnUntilFull ();
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube (transform.position, new Vector3 (width, height, 0));
	}

	// Update is called once per frame
	void Update () {
		// Direction formation is moving in
		if (movingRight) {
			transform.position += Vector3.right * moveSpeed * Time.deltaTime;
		} else {
			transform.position += Vector3.left * moveSpeed * Time.deltaTime;
		}

		// Check if formation is outside the playspace
		float formationRightEdge = transform.position.x + width / 2;
		float formationLeftEdge = transform.position.x - width / 2;

		if (formationLeftEdge < xmin) {
			movingRight = true;
		} else if (formationRightEdge > xmax) {
			movingRight = false;
		}

		if (AllMembersDead ()) {
			Debug.Log ("Empty Formation");
			SpawnUntilFull();
		}

	}

	void SpawnUntilFull() {
		Transform freePosition = NextFreePosition ();

		if (freePosition) {
			GameObject enemy = Instantiate (enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePosition; 	
		}

		// Only call again if there's another open position
		if (NextFreePosition ()) {
			Invoke ("SpawnUntilFull", spawnDelay);
		}
	}

	Transform NextFreePosition() {
		foreach (Transform childPositionGameObject in transform) {
			// Return a position gameobject if there is no child
			if (childPositionGameObject.childCount == 0) {
				return childPositionGameObject;
			}
		}
		return null;
	}

	bool AllMembersDead() {
		foreach(Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) {
				return false;
			}
		}
		return true;
	}
}
