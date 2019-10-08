using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JoyStick : MonoBehaviour {
    [System.NonSerialized]
    public Camera cam;
    private bool triggered = false;
    private Vector3 Orgpos;

	void Start () {
        cam = Camera.main;
        Orgpos = transform.position;
	}
	
	void Update () {
     /*   if (triggered)
        {
            transform.position = Vector3.Lerp(transform.position,Orgpos + Vector3.Normalize(Input.mousePosition - transform.position)*25,Time.deltaTime*10);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, Orgpos, Time.deltaTime * 10);
        }*/
	}
    public void onPress()
    {
        triggered = true;
        
    }
    public void onRelease()
    {
        triggered = false;
    }
    
}
