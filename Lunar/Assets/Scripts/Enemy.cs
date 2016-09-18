using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public Vector3 speed;
	public bool movingRight;
	public float multiplier = 10.0f;
	//TO DO: Add dying mechanisms
	// Use this for initialization
	void Start () {
		multiplier = 3.0f;
		if(transform.position.x > 0) {
			speed.x = multiplier * (Random.Range(-3,-5));

		} else if(transform.position.x < 0) {
			speed.x = multiplier * (Random.Range(3,5));
		}
	}

	// Update is called once per frame
	void Update () {
		Movement();
		Border();
	}

	void Movement() {
		transform.position += speed * Time.deltaTime;
	}

	void Border() {
		if(transform.position.x > 13.0f) {
			Destroy(gameObject);
		} else if(transform.position.x < -13.0f) {
			Destroy(gameObject);
		}
	}
	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			Debug.Log("Hit");
		}
	}
}
