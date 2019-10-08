using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゴールの処理
public class GoalBarier : MonoBehaviour {
    private int stages = 0;

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
