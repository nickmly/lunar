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
			transform.Rotate(new Vector3(0, 3.0f, 0));
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
