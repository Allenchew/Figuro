using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour {

    //UIボタンの処理
	public void RestartButton()
    {
        MapLoader.Instance.ResetState();
        Scene CurrentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(CurrentScene.name);
    }
    public void MainMButton()
    {
        MapLoader.Instance.ResetState();
        MapLoader.Instance.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene("scene2");
    }
    public void NextStage()
    {
        MapLoader.Instance.ResetState();
        MapLoader.Instance.StagesNum += 1;
        if (MapLoader.Instance.StagesNum == 4)
        {
            SceneManager.LoadScene("scene2");
        }
        else
        {
            Scene CurrentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(CurrentScene.name);
        }
    }
}
