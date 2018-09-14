using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class button2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public void OnClick()
    {
        SceneManager.LoadScene("scene2");
        
    }
    // Update is called once per frame
    void Update () {
		
	}
}
