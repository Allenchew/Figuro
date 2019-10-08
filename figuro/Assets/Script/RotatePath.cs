using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//回転通路の処理
public class RotatePath : MonoBehaviour {
    private bool DoneRotateR = true;
    private bool DoneRotateL = true;
    private bool FixSound = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(MapLoader.Instance.SwitchOne == false && gameObject.layer== 11)
        {
            if (DoneRotateR)
            {
                DoneRotateR = false;
                StartCoroutine(RotateCube(1));
                
            }
        }
        else if (MapLoader.Instance.SwitchTwo == false && gameObject.layer == 12)
        {
            if (DoneRotateL)
            {
                DoneRotateL = false;
                StartCoroutine(RotateCube(-1));
            }
        }
        if (MapLoader.Instance.SwitchOne == true && gameObject.layer == 11)
        {
           
                BoxCollider objCollider = GetComponent<BoxCollider>();
                objCollider.size = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (MapLoader.Instance.SwitchTwo == true && gameObject.layer == 12)
        {
                BoxCollider objCollider = GetComponent<BoxCollider>();
                objCollider.size = new Vector3(1.0f, 1.0f, 1.0f);
        }

        if (DoneRotateL && DoneRotateR && !FixSound)
        {
            GetComponent<AudioSource>().Play();
            FixSound = true;
        }



    }
    IEnumerator RotateCube(float RotateSide)
    {
        for(int i = 0; i < 90; i++)
        {
            transform.rotation *= Quaternion.Euler(RotateSide, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        if (RotateSide > 0) DoneRotateR = true;
        else DoneRotateL = true;
    }
}
