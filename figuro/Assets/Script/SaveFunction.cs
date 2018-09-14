using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class SaveFunction : MonoBehaviour {
    public static SaveFunction SaveAndLoad;

    public GameObject FocusMark;
    public GameObject MapNameField;
    public GameObject LoadMapField;
    public InputField MapNm;
    public Dropdown LoadMapSec;
    public Text inputPlace;
    public string LoadMapName;
    public Text debuglog;

    [System.NonSerialized]
    public bool EditMode =false;

    [System.NonSerialized]
    public Vector3[] TeleportTarget = new Vector3[]{ new Vector3(23, -13.8f, 25), new Vector3(24, -15.8f, 27),new Vector3(12,-22.8f,18),new Vector3(16,-26.8f,20),new Vector3(10,-26.8f,22),new Vector3(7,-22.8f,18), new Vector3(22, -22, 17)};

    [System.NonSerialized]
    public int[] EnemyNum = { 1, 2, 2 };

    private GameObject TempPos;
    private int LoadCounter = 0;
    private int debugTextCounter = 2;

    private void Awake()
    {
        if(SaveAndLoad == null)
        {
            SaveAndLoad = this;
        }else if( SaveAndLoad != this)
        {
            Destroy(SaveAndLoad);
        }
    }
    
    public void LoadMap()
    {
        if (MapLoader.Instance.EditorTrigger == false)
        {
            switch (MapLoader.Instance.StagesNum)
            {
                case 1:
                    LoadMapName = "tg1";
                    break;
                case 2:
                    LoadMapName = "tg4";
                    break;
                case 3:
                    LoadMapName = "tg2";
                    break;
                default:
                    Debug.Log("LoadMap Problem");
                    break;

            }
            LoadFile();
            
        }
    }
  
    public void ActiveMapNameField()
    {
        if (MapNameField.activeSelf == false)
        {
            FocusMark.SetActive(false);
            MapNameField.SetActive(true);
        }
    }
    public void CancelButton()
    {
        if (MapNameField.activeSelf == true)

        {
            FocusMark.SetActive(true);
            MapNameField.SetActive(false);
            MapNm.text = null;
        }
        else if (LoadMapField.activeSelf == true)
        {
            FocusMark.SetActive(true);
            LoadMapField.SetActive(false);
            LoadMapSec.ClearOptions();
        }
    }
    public void SaveFile()
    {
        GameObject Plyer = GameObject.FindGameObjectWithTag("Player");
        GameObject Enmy = GameObject.FindGameObjectWithTag("Enemy");
        FocusMark.SetActive(true);
        TempPos = GameObject.Find("ObjectTempPos");
        Save CollectPos = new Save();
        Transform[] allChildren = TempPos.transform.GetComponentsInChildren<Transform>();
        string destination =  "./Map/"+MapNm.text+".dat";
        
        Debug.Log(destination);
        FileStream file;
        CollectPos.ObjectPos.Add(new float[] { Plyer.transform.position.x, Plyer.transform.position.y, Plyer.transform.position.z });
        CollectPos.ObjectRot.Add(new float[] { Plyer.transform.rotation.x, Plyer.transform.rotation.y, Plyer.transform.rotation.z,Plyer.transform.rotation.w });
        CollectPos.ObjectType.Add(0);
        CollectPos.ObjectLayer.Add(Plyer.gameObject.layer);

      /*  CollectPos.ObjectPos.Add(new float[] { Enmy.transform.position.x, Enmy.transform.position.y, Enmy.transform.position.z });
        CollectPos.ObjectRot.Add(new float[] { Enmy.transform.rotation.x, Enmy.transform.rotation.y, Enmy.transform.rotation.z, Enmy.transform.rotation.w });
        CollectPos.ObjectType.Add(0);
        CollectPos.ObjectLayer.Add(Enmy.gameObject.layer);*/
        
        if (File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);
        int TeleporterCounter = 0;
        foreach (Transform childobj in allChildren)
        {
            if (childobj != TempPos.transform) { 
                int TypeNum;
                int Test = -1;

                if(int.TryParse(childobj.transform.tag,out Test))
                {
                    TypeNum = Test;
                }
                else
                {
                    TypeNum = 0;
                }
                if(TypeNum == 88)
                {
                    CollectPos.TeleporterNum.Add(TeleporterCounter);
                    TeleporterCounter++;
                }
                float[] Pos = { childobj.position.x, childobj.position.y, childobj.position.z };
                float[] Rot = { childobj.rotation.x, childobj.rotation.y, childobj.rotation.z, childobj.rotation.w };
                if(TypeNum == 13)
                {
                    Rot = new float[]{0,0,0,0};
                }
                CollectPos.ObjectPos.Add(Pos);
                CollectPos.ObjectRot.Add(Rot);

                CollectPos.ObjectType.Add(TypeNum);
                CollectPos.ObjectLayer.Add(childobj.gameObject.layer);
                
            }
        }
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, CollectPos);
        file.Close();
        MapNameField.SetActive(false);
        MapNm.text = null;
    }

    public void ActiveLoadMap()
    {
        if (LoadMapField.activeSelf == false) { LoadMapField.SetActive(true); FocusMark.SetActive(false); }
        var info = new DirectoryInfo("./Map");
        var fileinfo = info.GetFiles();
        List<string> DropOptions = new List<string>();
        foreach (FileInfo file in fileinfo) {
            DropOptions.Add(file.Name);
            Debug.Log(file.Name);
        }
        LoadMapSec.AddOptions(DropOptions);
    }
	public void LoadFile()
    {
        //FocusMark.SetActive(true);
        TempPos = GameObject.Find("ObjectTempPos");

            Save CollectPos = new Save();
            string destination = EditMode == true ? "./Map/" + LoadMapName + ".dat" : Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("/Android")) + "/Map/" + LoadMapName + ".dat";
            // string destination = "./Map/" + LoadMapName+ ".dat";//LoadMapSec.options[LoadMapSec.value].text;
            // string destination =Application.dataPath+ "./Map/" + LoadMapName + ".dat";//LoadMapSec.options[LoadMapSec.value].text;
            //string destination=  Application.persistentDataPath.Substring(0, Application.persistentDataPath.IndexOf("/Android")) + "/Map/" + LoadMapName + ".dat";
            FileStream file;
            if (File.Exists(destination)) file = File.OpenRead(destination);
            else
            {
                debuglog.text += "file not found\n";
                debuglog.text += Path.GetFullPath(destination);
                //Debug.Log("file not found");
                return;
            }

            BinaryFormatter bf = new BinaryFormatter();
            CollectPos = (Save)bf.Deserialize(file);
            file.Close();
        LoadCounter = 0;
        int TeleporterCounter = 0;
        int SwitchCounter = 0;
        foreach (float[] childobj in CollectPos.ObjectPos)
        {
            if (CollectPos.ObjectType[LoadCounter] == 0)
            {
                switch (LoadCounter)
                {
                    case 0:
                        GameObject Plyer = GameObject.FindGameObjectWithTag("Player");
                        Plyer.transform.position = new Vector3(CollectPos.ObjectPos[LoadCounter][0], CollectPos.ObjectPos[LoadCounter][1], CollectPos.ObjectPos[LoadCounter][2]);
                        Plyer.transform.rotation = new Quaternion(CollectPos.ObjectRot[LoadCounter][0], CollectPos.ObjectRot[LoadCounter][1], CollectPos.ObjectRot[LoadCounter][2], CollectPos.ObjectRot[LoadCounter][3]);
                        Plyer.layer = CollectPos.ObjectLayer[LoadCounter];
                        Debug.Log("load 0");
                        break;
                    /*case 1:
                        GameObject Enmy = GameObject.FindGameObjectWithTag("Enemy");
                        Enmy.transform.position = new Vector3(CollectPos.ObjectPos[LoadCounter][0], CollectPos.ObjectPos[LoadCounter][1], CollectPos.ObjectPos[LoadCounter][2]);
                        Enmy.transform.rotation = new Quaternion(CollectPos.ObjectRot[LoadCounter][0], CollectPos.ObjectRot[LoadCounter][1], CollectPos.ObjectRot[LoadCounter][2], CollectPos.ObjectRot[LoadCounter][3]);
                        Enmy.layer = CollectPos.ObjectLayer[LoadCounter];
                        Debug.Log("load 1");
                        break;*/
                    default:
                        
                        break;

                }
                
            }
            else
            {
                GameObject MapItem = Instantiate(Resources.Load(CollectPos.ObjectType[LoadCounter].ToString())) as GameObject;
                if(CollectPos.ObjectType[LoadCounter].ToString() == "88")
                {
                    MapItem.transform.name += "_" + CollectPos.TeleporterNum[TeleporterCounter].ToString();
                    TeleporterCounter++;
                }else if(CollectPos.ObjectType[LoadCounter].ToString() == "11")
                {
                    MapItem.transform.name += "_" + SwitchCounter;
                    SwitchCounter++;
                }
                MapItem.transform.position = new Vector3(CollectPos.ObjectPos[LoadCounter][0], CollectPos.ObjectPos[LoadCounter][1], CollectPos.ObjectPos[LoadCounter][2]);

                MapItem.transform.rotation = new Quaternion(CollectPos.ObjectRot[LoadCounter][0], CollectPos.ObjectRot[LoadCounter][1], CollectPos.ObjectRot[LoadCounter][2], CollectPos.ObjectRot[LoadCounter][3]);
                MapItem.transform.parent = TempPos.transform;
                MapItem.layer = CollectPos.ObjectLayer[LoadCounter];
                if (CollectPos.ObjectType[LoadCounter] > 90) break;
            }
                LoadCounter++;


        }
        debuglog.text += GameObject.Find("ObjectTempPos").transform.childCount+"\n";
        debuglog.text += Path.GetFullPath(".");
        LoadMapField.SetActive(false);
        LoadMapSec.ClearOptions();
       
    }

    public void LoggingState(string Logs)
    {
        debuglog.text += Logs + "\n";
        debugTextCounter++;
        if(debugTextCounter == 12)
        {
           int index = debuglog.text.IndexOf(System.Environment.NewLine);
            debuglog.text = debuglog.text.Substring(index + System.Environment.NewLine.Length);
            debugTextCounter--;
        }
    }
    public void LoadBinaryText()
    {
        Save CollectPos = new Save();
        string destination = "./Map/" + LoadMapSec.options[LoadMapSec.value].text;
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.Log("file not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        CollectPos = (Save)bf.Deserialize(file);
        file.Close();
        int i = 0;
        foreach (float[] childobj in CollectPos.ObjectPos)
        {
  
            inputPlace.text += CollectPos.ObjectType[i].ToString() + " ";
            if (CollectPos.ObjectType[i] > 90) break;
            i++;
        }
    }
}
