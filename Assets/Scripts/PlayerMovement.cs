using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : NetworkBehaviour
{
    public float movementSpeed, turnSpeed;
    private new Rigidbody rigidbody;
    public Transform bulletTransform;
    public GameObject bulletGameObject;

    void Start ()
    {
        rigidbody = GetComponent<Rigidbody>();	
	}
	
	
	void Update ()
    {
        if (isLocalPlayer)
        {
            Vector3 movement = transform.forward * Input.GetAxis("Vertical") * movementSpeed;
            Vector3 turn = transform.up * Input.GetAxis("Horizontal") * turnSpeed;
            rigidbody.angularVelocity = turn;
            rigidbody.velocity = movement;

            if (Input.GetButtonDown("Fire1"))
            {
                CmdFire();
            }
        }
    }

    [Command]
    void CmdFire()
    {
       GameObject go =  Instantiate(bulletGameObject, bulletTransform.transform.position, bulletTransform.transform.rotation);
       NetworkServer.Spawn(go);
    }
}

