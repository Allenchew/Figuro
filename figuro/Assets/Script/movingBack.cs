using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingBack : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition += new Vector3(0,0.8f,0);
        if (transform.localPosition.y > 1210)
            transform.localPosition = new Vector3(0, -3620, 0);
	}
}
