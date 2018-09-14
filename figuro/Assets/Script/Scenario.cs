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
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            FlowSpeed = 0.05f;
        }else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            FlowSpeed = 0.1f;
        }
	}
   /* IEnumerator OutPutChat()
    {
        for(int i = 0; i < 3; i++)
        {
            if (i == 1)
            {
                StartCoroutine("testzoom");
            }
            foreach (char A in test1[i])
            {
                chatbox1.text += A;
                yield return new WaitForSeconds(FlowSpeed);
            }
            yield return new WaitForSeconds(FlowSpeed*10);
            chatbox1.text = "";
        }
       
        gameObject.SetActive(false);
    }
    IEnumerator testzoom()
    {
            for (int i = 0; i < 20; i++)
            {
                cam.orthographicSize = Mathf.Lerp(11, 5, (float)i / 20);
                StgCam.orthographicSize = Mathf.Lerp(11, 5, (float)i / 20);
                StgCam2.orthographicSize = Mathf.Lerp(11, 5, (float)i / 20);
            cam.transform.position += new Vector3(0.1f, 0, -0.1f);
            yield return new WaitForSeconds(0.04f);
            }
            
        yield return new WaitForSeconds(1.0f);
        MapLoader.Instance.startMov = true;
        for (int i = 20; i >0; i--)
        {
            cam.orthographicSize = Mathf.Lerp(11, 5, (float)i / 20);
            StgCam.orthographicSize = Mathf.Lerp(11, 5, (float)i / 20);
            StgCam2.orthographicSize = Mathf.Lerp(11, 5, (float)i / 20);
            cam.transform.position -= new Vector3(0.1f, 0, -0.1f);
            yield return new WaitForSeconds(0.04f);
        }

    }*/
}
