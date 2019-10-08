using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//マップの編集機能
public class AddItem : MonoBehaviour {
    
    public int ItemNo = 1;
    public GameObject Items;
    public Vector3 AddBlockPos = new Vector3(0, 0, 0);
    private GameObject TempPos;
	
	//ナンバーで入れるキューブタイプを変える
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

    //キューブの生成処理
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
