using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//データを保存する容器を作る
[System.Serializable]
public class Save{
    public List<float[]> ObjectPos = new List<float[]>();
    public List<float[]> ObjectRot = new List<float[]>();
    public List<int> ObjectType = new List<int>();
    public List<int> ObjectLayer = new List<int>();
    public List<int> TeleporterNum = new List<int>();
}
