using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour {
    //ゲームリセット、タイトルに戻る
    public void ClickedButton()
    {
        var GM = GameObject.Find("MapLoader");
        Destroy(GM);
        SceneManager.LoadScene("scene1");
    }
}
