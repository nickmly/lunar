using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

	public bool pickedUp = false;
	public BoxCollider bC;
	// Use this for initialization
	void Start () {
		bC = gameObject.GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		Rotate();
	}

	void Rotate(){
		if(!pickedUp) {
			transform.Rotate(new Vector3(-1.0f, 1.0f, 1.0f));
		}
	}

	void PickUp() {
		pickedUp = false;
		Destroy(gameObject);
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Player") {
			PickUp();
		}
	}
}
