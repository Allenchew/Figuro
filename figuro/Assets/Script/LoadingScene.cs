using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingScene : MonoBehaviour {
    AsyncOperation Nowloading;
    public Image Progressbar;
    private const float LoadingMax = 0.9f;

	void Start () {
        StartCoroutine("Loading");
	}
	
    IEnumerator Loading()
    {
        yield return new WaitForSeconds(1f);
        Nowloading = SceneManager.LoadSceneAsync("tg2");
        Nowloading.allowSceneActivation = false;
        var ProgressCube = Progressbar.GetComponent<CanvasGroup>();
        while (!Nowloading.isDone)
        {
            ProgressCube.alpha = Nowloading.progress;
            yield return new WaitForSeconds(0.01f);
            if(Nowloading.progress >= LoadingMax)
            {
                ProgressCube.alpha = 1.0f;
                MapLoader.Instance.GetComponent<AudioSource>().Stop();
                Nowloading.allowSceneActivation = true;
            }
            yield return null;
            
        }

    }
}
