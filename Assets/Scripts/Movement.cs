using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private Rigidbody rigidbody;
    public float speed;

	void Start ()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = transform.forward * speed;
	}
	
}
