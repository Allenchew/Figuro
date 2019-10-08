using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//カメラをマップを見るモードとプレイモードの切り替え処理
public class SwitchCamMode : MonoBehaviour {
    [System.NonSerialized]
    public Camera cam;

    public GameObject MapModeScreen;
    private GameObject Acam;
    private GameObject Bcam;
    private bool Zooming = false;
    
	// Use this for initialization
	void Start () {
        cam = Camera.main;
        Acam = cam.transform.GetChild(0).gameObject;
        Bcam = cam.transform.GetChild(1).gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchMode()
    {
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>()._StopCameraZoom)
        {
            return;
        }

        if (!MapLoader.Instance.MapMode && !Zooming)
        {
            Zooming = true;
            StartCoroutine(ZoomInOut(0.2f));
            GameObject.Find("Outer").transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            GameObject.Find("Inner").transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            MapModeScreen.SetActive(true);
            MapLoader.Instance.MapMode = true;
        }
        else if(MapLoader.Instance.MapMode && !Zooming)
        {
            Zooming = true;
            StartCoroutine(ZoomInOut(-0.2f));
            GameObject.Find("Outer").transform.localScale = new Vector3(1f, 1f, 1f);
            GameObject.Find("Inner").transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
            MapModeScreen.SetActive(false);
            MapLoader.Instance.MapMode = false;
        }
    }
    IEnumerator ZoomInOut(float Increasement)
    {

        for(int i = 0; i < 50; i++)
        {
            cam.orthographicSize += Increasement;
            Acam.GetComponent<Camera>().orthographicSize += Increasement;
            Bcam.GetComponent<Camera>().orthographicSize += Increasement;
            yield return new WaitForFixedUpdate();
        }
        Zooming = false;
    }
}
