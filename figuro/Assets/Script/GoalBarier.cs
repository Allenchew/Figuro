using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBarier : MonoBehaviour {
    private int stages = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(stages ==0 && MapLoader.Instance.SwitchOne == true)
        {
            transform.position += new Vector3(0, 0, -4);
            stages++;
        }else if (stages == 1 && MapLoader.Instance.SwitchTwo == true)
        {
            transform.position += new Vector3(0, 0, -4);
            stages++;
        }

    }
}
