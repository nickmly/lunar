using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;

    void Start()
    {

    }


	void Update()
    {
        transform.position = target.position + offset;
    }
}
