using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class SmoothFollow : MonoBehaviour
{

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    private Camera cam;
	private float outOfBoundsLeft = -41.1f;
	private float outOfBoundsRight = 1000.0f;

    void Start()
    {
        cam = GetComponent<Camera>();
    }
    void Update()
    {
        if (target)
        {
            Vector3 point = cam.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            Vector3 final = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
			if(final.x > outOfBoundsLeft && final.x < outOfBoundsRight) {
				if(final.y < 0) {
					transform.position = new Vector3(final.x, 0, -75.0f);
				} else {
					transform.position = new Vector3(final.x, final.y, -75.0f);
				}
			}
        }
    }
}