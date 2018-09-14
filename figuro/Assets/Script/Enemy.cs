using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "81":
                gameObject.layer = 0;
                break;
            case "82":
                gameObject.layer = 8;
                break;
            case "83":
                gameObject.layer = 10;
                break;
            case "84":
                // OnSideCurve = true;
                break;
            default:
                Debug.Log("Touch Something Else");
                break;
        }
    }
}
