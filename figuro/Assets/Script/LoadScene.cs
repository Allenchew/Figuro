using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadFirstStage()
    {
        SceneManager.LoadScene("LoadingScene");
        MapLoader.Instance.StagesNum = 1;
    }
    public void LoadSecondStage()
    {
        SceneManager.LoadScene("LoadingScene");
        MapLoader.Instance.StagesNum = 2;
    }
    public void LoadThirdStage()
    {
        SceneManager.LoadScene("LoadingScene");
        MapLoader.Instance.StagesNum = 3;
    }
    
}
