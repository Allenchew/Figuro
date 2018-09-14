using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMode : MonoBehaviour {
    public GameObject MapManager;
    public GameObject FocusMark;
    public GameObject ButtonCanvas;
    public GameObject MapNameField;
    public GameObject LoadMapField;
    // Use this for initialization
   
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(MapLoader.Instance.EditorTrigger == true && MapManager.activeSelf == false)
        {
            FocusMark.SetActive(true);
            ButtonCanvas.SetActive(true);
            MapManager.SetActive(true);
           // MapNameField.SetActive(true);
          //  LoadMapField.SetActive(true);
        }else if(MapLoader.Instance.EditorTrigger == false && MapManager.activeSelf == true)
        {
            FocusMark.SetActive(false);
            ButtonCanvas.SetActive(false);
            MapManager.SetActive(false);
            MapNameField.SetActive(false);
            LoadMapField.SetActive(false);
        }
	}
}
