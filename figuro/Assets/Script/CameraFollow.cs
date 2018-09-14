using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    public AudioClip Stage1;
    public AudioClip Stage2;
    public AudioClip Stage3;
    public bool _StopCameraZoom = false;

    private Vector3[][] CameraMovPos = new Vector3[][] {  new Vector3[] {   new Vector3( 0, 0, 0 )},
                                                          new Vector3[] {   new Vector3(-25,20,-36),    new Vector3(-25,20,-36) },
                                                          new Vector3[] {   new Vector3(-4,17,-13) ,    new Vector3(-1,17,-13) } };

    private Vector3 orgpos;
    private Vector3 Tarpos;
    private int StageNo;

    private void Awake()
    {
       
            AudioClip[] BgmName = { Stage1, Stage2,Stage3 };
            var A = GetComponent<AudioSource>();
            A.clip = BgmName[MapLoader.Instance.StagesNum - 1];
            A.Play();
        
    }

    // Use this for initialization
    void Start()
    {
        StageNo = MapLoader.Instance.StagesNum - 1;
        Player = GameObject.FindGameObjectWithTag("Player");
        transform.position = transform.forward * 50;

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        //(-26,31,-17)

        if (MapLoader.Instance.CameraPlaceOne && !MapLoader.Instance.SwitchOne)
        {
            MapLoader.Instance.startMov = false;
            StartCoroutine(StarChange(CameraMovPos[StageNo][0], true, false, false, false));
        }

        if (MapLoader.Instance.CameraPlaceTwo && !MapLoader.Instance.SwitchTwo)
        {
            MapLoader.Instance.startMov = false;
            StartCoroutine(StarChange(CameraMovPos[StageNo][1], true, true, false, false)); 
        }
        if(MapLoader.Instance.CameraPlaceThree)
        {
            MapLoader.Instance.CameraPlaceThree = false;
            StartCoroutine("PresetCamPos");
        }

        if (MapLoader.Instance.startMov && !MapLoader.Instance.MapMode)
        {
            transform.position = Vector3.Lerp(transform.position, Player.transform.position - transform.forward * 50, Time.deltaTime * 1.5f);
        }
        
        if (MapLoader.Instance._gameOver)
        {
            transform.position = Vector3.Lerp(transform.position, Player.transform.position - transform.forward * 50, Time.deltaTime * 1.5f);
            GetComponent<Camera>().orthographicSize =
            transform.GetChild(0).GetComponent<Camera>().orthographicSize =
            transform.GetChild(1).GetComponent<Camera>().orthographicSize =
            Mathf.Lerp(GetComponent<Camera>().orthographicSize, 4.5f, Time.deltaTime * 2);
        }

    }
    IEnumerator PresetCamPos()
    {
        yield return new WaitForSeconds(2f);
        transform.position = new Vector3(-26, 31, -17);
    }
    IEnumerator StarChange(Vector3 MovePos, bool _switchOne, bool _switchTwo, bool _placeOne, bool _placetwo)
    {
        _StopCameraZoom = true;
        transform.position = Vector3.Lerp(transform.position, MovePos, Time.deltaTime * 2f);
        yield return new WaitForSeconds(1.5f);
        GetComponent<Camera>().orthographicSize =
        transform.GetChild(0).GetComponent<Camera>().orthographicSize =
        transform.GetChild(1).GetComponent<Camera>().orthographicSize =
        Mathf.Lerp(GetComponent<Camera>().orthographicSize, 8.5f, Time.deltaTime * 2);
        yield return new WaitForSeconds(1.5f);

        MapLoader.Instance.SwitchOne = _switchOne;
        MapLoader.Instance.SwitchTwo = _switchTwo;
        yield return new WaitForSeconds(2f);

        GetComponent<Camera>().orthographicSize =
        transform.GetChild(0).GetComponent<Camera>().orthographicSize =
        transform.GetChild(1).GetComponent<Camera>().orthographicSize =
        Mathf.Lerp(GetComponent<Camera>().orthographicSize, 11f, Time.deltaTime * 2);

        yield return new WaitForSeconds(1f);


        MapLoader.Instance.startMov = true;
        MapLoader.Instance.CameraPlaceTwo = _placeOne;
        MapLoader.Instance.CameraPlaceTwo = _placetwo;
        yield return new WaitForSeconds(1.5f);
        _StopCameraZoom = false;
    }

}
