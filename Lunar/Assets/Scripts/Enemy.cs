using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public Vector3 speed;
	public bool movingRight;
	public float multiplier = 10.0f;
	public Vector3 target;
	//TO DO: Add dying mechanisms
	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform.position;
		multiplier = 3.0f;
		if(transform.position.x > target.x) {
			speed.x = multiplier * (Random.Range(-1,-3));
		} else {
			speed.x = multiplier * (Random.Range(1,3));
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
		if(transform.position.x > target.x + 25.0f) {
			Destroy(gameObject);
		} else if(transform.position.x < target.x -25.0f) {
			Destroy(gameObject);
		}
	}
	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			Debug.Log("Hit");
		} else if(other.tag == "Land") {
			Destroy(gameObject);
		}
	}
}
