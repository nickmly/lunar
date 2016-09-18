using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float offset;

    void Start()
    {

    }
    void Update()
    {
		transform.position = new Vector3(transform.position.x, target.position.y + offset, transform.position.z);
    }
}