using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

//マップのフラグの初期化
public class MapLoader : MonoBehaviour {
    public static MapLoader Instance;
    public bool startMov = false;
    public bool EditorTrigger = false;
    public int StagesNum = 1;
    public bool MapMode = false;
    
    [System.NonSerialized]
    public bool Rewind = false;
    [System.NonSerialized]
    public bool SwitchOne = false;
    [System.NonSerialized]
    public bool SwitchTwo = false;
    [System.NonSerialized]
    public bool CameraPlaceOne = false;
    [System.NonSerialized]
    public bool CameraPlaceTwo = false;
    [System.NonSerialized]
    public bool CameraPlaceThree = false;
    [System.NonSerialized]
    public bool FlashSwitchOne = false;
    [System.NonSerialized]
    public bool FlashSwitchTwo = false;
    [System.NonSerialized]
    public bool _gameOver = false;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(Instance);
        }
    }

    public void ResetState()
    {
        MapLoader.Instance.Rewind = false;
        MapLoader.Instance.startMov = false;
        MapLoader.Instance.SwitchOne = false;
        MapLoader.Instance.SwitchTwo = false;
        MapLoader.Instance.CameraPlaceOne = false;
        MapLoader.Instance.CameraPlaceTwo = false;
        MapLoader.Instance.FlashSwitchOne = false;
        MapLoader.Instance.FlashSwitchTwo = true;
        MapLoader.Instance._gameOver = false;
        MapMode = false;
    }
	
}
