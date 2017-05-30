using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectObject;
    private bool selsected;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(selsected == false && Input.GetAxisRaw("Vertical")!=0)
        {
            eventSystem.SetSelectedGameObject(selectObject);
            selsected = true;
        }
	}

    private void OnDisable()
    {
        selsected = false;
    }
}
