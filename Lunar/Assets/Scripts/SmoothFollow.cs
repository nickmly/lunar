using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class SmoothFollow : MonoBehaviour
{

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
	public float offsetX, offsetY;
    private Camera cam;
	private float outOfBoundsLeft = -41.1f;
	private float outOfBoundsRight = 1000.0f;

    void Start()
    {
		offsetY = 18.0f;
		offsetX = 1.25f;
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        if (target)
        {
			if(target.position.y < 0) {
				transform.position = new Vector3(target.position.x + offsetX, 0 + offsetY, -75.0f);
			} else {
				transform.position = new Vector3(target.position.x+offsetX, target.position.y + offsetY, -75.0f);
			}
        }
    }
}