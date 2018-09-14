using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFire : MonoBehaviour {

    public float _coldDownTime = 0;
    public float WaitTime;
    public float CanRunTime;

    GameObject Futa;
    bool MoveUp;
    bool MoveDown;
    bool _Fire;
    bool ColdDown;
    float _startTime = 0;
    float _moveNum = 0.009f;
    float countColdDown = 0;
    
    Vector3 UpfinalStop;
    Vector3 DownFinalStop;


    // Use this for initialization
    void Start () {
        Futa = this.gameObject.transform.GetChild(0).gameObject;

        UpfinalStop.y = Futa.transform.localPosition.y + _moveNum;
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        
        gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().collision.SetPlane(0, player);
        foreach(Transform childobj in gameObject.transform)
        {
            childobj.gameObject.layer = gameObject.layer;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (MapLoader.Instance._gameOver)
        {
            return;
        }

        if (_startTime < 5 && !MoveUp && !_Fire)
        {
            _startTime += 1*Time.deltaTime;
        }
        else if (_startTime >= 5 && !MoveUp && !_Fire && !ColdDown)
        {
            _startTime = 5;
            MoveUp = true;
            countColdDown = 0;
        }
        if (MoveUp)
        {
            FutaMoveUp();
        }
        if (MoveDown)
        {
            FutaMoveDown();
        }

        if (ColdDown)
        {
            _startTime = 0;
            countColdDown+= 1*Time.deltaTime;
        }
        if ( countColdDown>= _coldDownTime)
        {
            countColdDown = _coldDownTime;
            ColdDown = false;
        }

	}

    private  void FutaMoveUp()
    {
        if (MoveUp)
        {
            if (Futa.transform.localPosition.y >UpfinalStop.y )
            {
                StartCoroutine("StartFire");
                DownFinalStop.y = Futa.transform.localPosition.y-_moveNum;
            }
            else
            {
                Futa.transform.position += new Vector3(0, 0.8f, 0) * Time.deltaTime;
            }
        }
    }

    IEnumerator StartFire()
    {
        _Fire = true;
        yield return new WaitForSeconds(WaitTime);
       
        if (MoveUp)
        {
            this.gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Play();
            GetComponent<AudioSource>().Play();
            StartCoroutine("StopFire");
        }
        MoveUp = false;
        yield return new WaitForSeconds(CanRunTime);
        MoveDown = true;
    }

    private void FutaMoveDown()
    {
        if (MoveDown)
        {
            if (Futa.transform.localPosition.y<DownFinalStop.y)
            {
                _Fire = false;
                ColdDown = true;
                MoveDown = false;
            }
            else
            {
                Futa.transform.position -= new Vector3(0, 0.8f, 0) * Time.deltaTime;
            }
        }
        
    }
    private IEnumerator StopFire()
    {
        yield return new WaitForSeconds(2.6f);

        GetComponent<AudioSource>().Stop();

    }




}
