using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RotateTowards : MonoBehaviour {
	public Transform t;
	public Transform c;
	public RectTransform self;
	public Text distanceText;
	Vector3 target;
	Vector3 current;
	public float distance;
	public float theta;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		target = t.position;
		current = c.position;
		distance = Vector3.Distance(target, current);
		theta = (Mathf.Atan2((target-current).y, (target-current).x) * Mathf.Rad2Deg);
		Debug.Log(theta*Mathf.Rad2Deg);
		self.rotation = Quaternion.Euler(0.0f, 0.0f, theta-90);
		distanceText.text = ((int)distance).ToString();

	}
}
