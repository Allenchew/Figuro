using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//マップの処理で、落ちるキューブを削除する
public class ClearDropCube : MonoBehaviour {
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11)
        {
            Destroy(other.gameObject);
        }
    }
}
