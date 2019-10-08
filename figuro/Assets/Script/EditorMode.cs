using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//マップの編集モードに変わる
public class EditorMode : MonoBehaviour {
    public GameObject MapManager;
    public GameObject FocusMark;　　//物を入れる場所を示す
    public GameObject ButtonCanvas;
    public GameObject MapNameField;　
    public GameObject LoadMapField;

    //起動編集モードに関係あるもの
	void Update () {
		if(MapLoader.Instance.EditorTrigger == true && MapManager.activeSelf == false)
        {
            FocusMark.SetActive(true);
            ButtonCanvas.SetActive(true);
            MapManager.SetActive(true);
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
