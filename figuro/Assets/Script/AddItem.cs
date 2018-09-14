using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour {
    public int ItemNo = 1;
    public GameObject Items;
    public Vector3 AddBlockPos = new Vector3(0, 0, 0);
    private GameObject TempPos;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ItemNo = 1;
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ItemNo = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ItemNo = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ItemNo = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ItemNo = 5;
        }
    }
    public void ClickedAddButton()
    {
        if (TempPos == null) TempPos = GameObject.Find("ObjectTempPos");
        Debug.Log("Add");
        Debug.Log(TempPos.transform.name);
        GameObject ItemA = Instantiate(Resources.Load(ItemNo.ToString())) as GameObject;
        ItemA.transform.position = AddBlockPos;
        ItemA.transform.parent = TempPos.transform;
    }
}
