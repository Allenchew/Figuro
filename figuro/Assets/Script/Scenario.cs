using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scenario : MonoBehaviour {

    private string[] test1 = { "早く逃げないと", "あそこだ。\nあそこから出られる。", "何で知ってるの？" };
    private float FlowSpeed = 0.1f;
    
    public Camera cam;
    public Camera StgCam;
    public Camera StgCam2;
    private int StagesNo;
    
	// Use this for initialization
	void Start () {
        SaveFunction.SaveAndLoad.LoadMap();
        StagesNo = MapLoader.Instance.StagesNum;
        
        cam = Camera.main;

        cam.transform.position += cam.transform.forward * 100;
        switch (StagesNo)
        {
            case 1:
                MapLoader.Instance.startMov = true;
                break;
            case 2:
                MapLoader.Instance.startMov = true;
                break;
            case 3:
                MapLoader.Instance.startMov = true;
                break;
            default:
                Debug.Log("StagesNo Problem");
                break;

        }
        
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FlowSpeed = 0.05f;
        }else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            FlowSpeed = 0.1f;
        }
	}
}
