using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDropCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11)
        {
            Destroy(other.gameObject);
        }
    }
}
