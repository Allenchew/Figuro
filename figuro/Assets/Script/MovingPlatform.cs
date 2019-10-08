using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ステージの移動踏み場の処理
public class MovingPlatform : MonoBehaviour {
    private float stepper = 0.1f;
    private float reverser = -1;
    private bool Stopper = false;
    private bool Running = false;
    private bool PlayedSound = false;

	void Start () {
		if(MapLoader.Instance.StagesNum != 2)
        {
            Stopper = true;
        }
	}
	
	void Update () {
		if(!Stopper &&(MapLoader.Instance.SwitchTwo == false && !Running))
        {
            Running = true;
            reverser *= -1;
            StartCoroutine("movingPlat");
        }
        if (!Stopper && (MapLoader.Instance.SwitchTwo == true && !Running) && !PlayedSound)
        {
            PlayedSound = true;
            if(gameObject.tag != "9")
            GetComponent<AudioSource>().Play();
        }
    }
    IEnumerator movingPlat()
    {
        stepper *= -1;
        yield return null;
        for (int i = 0; i < 60; i++)
        {
            transform.position += new Vector3(stepper*reverser, 0, 0);
            yield return new WaitForSeconds(0.04f);
            if (i == 29) stepper *= -1;
        }
        Running = false;
    }
}
