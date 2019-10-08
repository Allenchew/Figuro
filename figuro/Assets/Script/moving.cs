using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//マップ編集モードにマークを移動する処理
//マークにある場所をキューブ入れて、マップを作る
public class moving : MonoBehaviour {
    public GameObject MovingObject;
    private Camera cam;

    private bool MovingMode = false;

    void Start () {
        gameObject.transform.GetComponent<CanvasGroup>().alpha = 0;
        cam = Camera.main;
    }

	void Update () {
        if (MapLoader.Instance.EditorTrigger == true)
        {
            if (cam == null) cam = Camera.main;

            if (Input.GetMouseButtonDown(0) == true)
            {
                Debug.Log("clicked");
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    MovingObject = hit.transform.gameObject;
                    MovingMode = true;
                    gameObject.transform.GetComponent<CanvasGroup>().alpha = 1;
                }
            }
            if (MovingMode && MovingObject!= null)
            {
                transform.position = MovingObject.transform.position;
                if (Input.GetKeyDown(KeyCode.W) == true)
                {
                    MovingObject.transform.position += new Vector3(0, 0, 1);
                }
                else if (Input.GetKeyDown(KeyCode.S) == true)
                {
                    MovingObject.transform.position -= new Vector3(0, 0, 1);
                }
                else if (Input.GetKeyDown(KeyCode.A) == true)
                {
                    MovingObject.transform.position -= new Vector3(1, 0, 0);
                }
                else if (Input.GetKeyDown(KeyCode.D) == true)
                {
                    MovingObject.transform.position += new Vector3(1, 0, 0);
                }
                else if (Input.GetKeyDown(KeyCode.Q) == true)
                {
                    MovingObject.transform.rotation *= Quaternion.Euler(0, 90, 0);
                }
                else if (Input.GetKeyDown(KeyCode.E) == true)
                {
                    MovingObject.transform.rotation *= Quaternion.Euler(0, -90, 0);
                }
                else if (Input.GetKeyDown(KeyCode.R) == true)
                {
                    MovingObject.transform.position += new Vector3(0, 1, 0);
                }
                else if (Input.GetKeyDown(KeyCode.F) == true)
                {
                    MovingObject.transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
        
    }


}
