using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    //敵の映る階層を変える
    //不思議図形なので、敵をマップの上にあるように見せるため
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "81":
                gameObject.layer = 0;
                break;
            case "82":
                gameObject.layer = 8;
                break;
            case "83":
                gameObject.layer = 10;
                break;
            case "84":
                // OnSideCurve = true;
                break;
            default:
                Debug.Log("Touch Something Else");
                break;
        }
    }
}
