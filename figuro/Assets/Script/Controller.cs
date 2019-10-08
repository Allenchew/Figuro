using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    #region 初期化
    #region 敵関連初期化
    public GameObject EnemyObj;

    private List<Vector3[]> EnemyStartingPos = new List<Vector3[]>();       // use to store Enemy object starting position and location
    private List<Vector3[]> EnemyStartingRot = new List<Vector3[]>();
    private List<GameObject> SetEnemy = new List<GameObject>();               // use to contain cloned enemy object and apply position on it
    private List<int[][]> EnemyProcedure = new List<int[][]>{   new int [][] { new int[] {2, 0, 6, 1, 180 }},   //Use to set up Enemy Object moving procedure, index 0 stands for how many steps
                                                                new int [][] { new int[] {12,0,3,1,90,0,8,1,90,0,3,1,180,0,3,1,-90,0,8,1,-90,0,3,1,180},new int[]{2,0,6,1,180} }, //odd number index refer to line 552-568
                                                                new int [][] { new int[] {8,0,4,1,90,0,4,1,180,0,4,1,-90,0,4,1,180}, new int[] {4,0,7,1,-90,0,6,1,-90} }};// new int[]{Total Steps, 1st step, How much to move for 1st, 2nd Step,How much to move for 2nd, etc}
    private int[] EnemyStepCounter;         // To count if Enemy finish current steps
    private int[] ProcedureCounter;         // To count which procedure Enemy object at
    private bool EnemyStartMove = false;    // Trigger to start Move enemy object
    private bool NewMove = false;           // Use to trigger Enemy sliding movement (scrap)>> trigger EnemyNewMove and start new moveset
    private bool EnemyNewMove = false;      // For Enemy object that will change their movement pattern
    private bool enemMoving = false;        // The animation that Enemy object move from a place to another place (moving or not)
    private int EnemyCount; //Use to Set the number of enemies in stage
    private bool StepReverser = false;        //Use to reverse Enemy object movement (scrap)
    #endregion
    #region　その他の初期化
    public GameObject[] TextImage = new GameObject[2];
    public Rigidbody Prb;                                //Player Physics 
    public GameObject JoyButton;
    private Camera cam;//JoyStick as a button
    
    public float DeadCamTime = 2;                           // Delay time for GameOver screen to show up
    private int stageNo; // Use to get which stage is loaded from MapLoader
    private int DeathCount = 2;             // Chances for Player to respawn from stage 3 Trap Zone
    private Vector3 JoyOrgpos;  // Use to record joystick original position
    #endregion
    #region 当たり判定の保存
    private bool[] AbleMovFront = { false, false, false, false }; // 2:forward 3:backward 1:Left 0:Right  Use to check if the direction is movable
    private bool[] OnSideCurve = { false, false, false, false };  // Use to mark if there is Curve movement in different direction
    private bool[] OnInvertCurve = { false, false, false, false }; //Use to mark if there is invert Curve movement in different direction
    private bool[] Upvector = { false, false, false, false };
    #endregion
    #region フラグ
    private bool StartTeleport = false; //Decide if player should change it's position
    private string TeleporterName = ""; // Decide which object's position Player should move to
    private bool OnMoving = false; //Check if Player is moving Curve to next position
    private bool OnNormMove = false;//Check if Player is moving straight to next position
    private bool Death = false; // Mark if Player die
    private bool OnstayTrigger = false;  //Use to prevent OnStayTrigger function run more than once
    private bool Joytrigger = false; // If joystick is pressed
    private bool explodecontrol = false; // use to control Explode function to function differently
    #endregion
    

    //マップ初期化
    void Start()
    {
        stageNo = MapLoader.Instance.StagesNum - 1;   // Read in Stage Number
        EnemyStartingPos.Add(new Vector3[] { new Vector3(1, 0.5f, 5.5f) });     //Set up Enemy transform
        EnemyStartingRot.Add(new Vector3[] { new Vector3(0, 180, 0) });
        EnemyStartingPos.Add(new Vector3[] { new Vector3(6, -2, 6.5f), new Vector3(-1, -10.5f, 5) });
        EnemyStartingRot.Add(new Vector3[] { new Vector3(90, -90, 90), new Vector3(0, 90, 0) });
        EnemyStartingPos.Add(new Vector3[] { new Vector3(11, -23.5f, 22), new Vector3(10, -27.5f, 17) });
        EnemyStartingRot.Add(new Vector3[] { new Vector3(0, 180, 0), new Vector3(0, 180, 0) });
        EnemyCount = SaveFunction.SaveAndLoad.EnemyNum[MapLoader.Instance.StagesNum - 1]; //Read Enemy count that is preset
        ProcedureCounter = new int[EnemyCount]; // Define ProcedureCounter
        EnemyStepCounter = new int[EnemyCount]; // Define StepCounter
        for (int i = 0; i < EnemyStepCounter.Length; i++)//Reset Counter
        {
            EnemyStepCounter[i] = 0;
            ProcedureCounter[i] = 0;
        }
        Prb = gameObject.GetComponent<Rigidbody>();  //Get Rigidbody
        FindMovable(); // Check collider  Refers to Line 572-693
        cam = Camera.main; // Set Main Camera
        JoyOrgpos = JoyButton.transform.position; // Set joystick position
        EnemyGenerate(); // generate enemy from prefab Refers to Line 528-537
    }
    #endregion

    void Update() {
        //マップの爆発処理とプレイヤーの位置を再配置
        if (!explodecontrol &&( gameObject.transform.position.y <= 1.2f && !MapLoader.Instance.startMov && Prb.useGravity ))
        {
            transform.position = new Vector3(2, 1.2f,11); //Use to repositioning Player after falling in stage 2
            MapLoader.Instance.startMov = true; // Use to Control if Player can move
            Prb.useGravity = false; // turn off Gravity
            Prb.isKinematic = true;
            Prb.constraints = RigidbodyConstraints.None;
        }
        #region　プレイヤーを操作する
        if (!MapLoader.Instance.MapMode) // if is not in Map mode
        {
            if (MapLoader.Instance._gameOver) // If player dies
            {
                return;
            }
            #region タブレットモード
            if (SaveFunction.SaveAndLoad.EditMode ==false) // if its not for pc editing mode
            {
                if (Joytrigger) // if joystick pressed
                {
  
                    Vector3 RangeOfvectors = Input.mousePosition - JoyOrgpos; 
                    Vector3 compareTouch = Vector3.Normalize(RangeOfvectors);
                    if (Mathf.Abs(RangeOfvectors.x) + Mathf.Abs(RangeOfvectors.y) > 20)
                    {
                        JoyButton.transform.position = Vector3.Lerp(JoyButton.transform.position, JoyOrgpos + compareTouch * 35, Time.deltaTime * 20);

                        if (!OnNormMove && !OnMoving && MapLoader.Instance.startMov && (!TextImage[0].activeSelf && !TextImage[1].activeSelf))
                        {
                            FindMovable();
                            int Switcher = 4;
                            string CurveSide = "";
                            Vector3 CurrentJoy = Vector3.Normalize(JoyButton.transform.position - JoyOrgpos);
                            #region マップ上プレイヤーを曲がる処理
                            if (CompareValue(CurrentJoy.x, -0.5f, 0.5f) && CompareValue(CurrentJoy.y, 0.5f, 1f))
                            {
                                if (transform.rotation.eulerAngles.z > 180f)
                                {
                                    Switcher = 1;
                                    CurveSide = "L";

                                }
                                else if (transform.rotation.eulerAngles.x > 180f)
                                {
                                    Switcher = 2;
                                    CurveSide = "F";
                                }
                            }
                            else if (CompareValue(CurrentJoy.x, -0.5f, 0.5f) && CompareValue(CurrentJoy.y, -1f, -0.5f))
                            {
                                if (transform.rotation.eulerAngles.z > 180f)
                                {
                                    Switcher = 0;
                                    CurveSide = "R";
                                }
                                else if (transform.rotation.eulerAngles.x > 180f)
                                {
                                    Switcher = 3;
                                    CurveSide = "B";
                                }
                                
                            }
                            else if (CompareValue(CurrentJoy.x, 0.5f, 1) && CompareValue(CurrentJoy.y, 0.5f, 1f))
                            {
                                if (transform.rotation.eulerAngles.x < 180f)
                                {
                                    Switcher = 2;
                                    CurveSide = "F";
                                }
                            }
                            else if (CompareValue(CurrentJoy.x, -1, -0.5f) && CompareValue(CurrentJoy.y, -1, -0.5f))
                            {
                                if (transform.rotation.eulerAngles.x < 180f)
                                {
                                    Switcher = 3;
                                    CurveSide = "B";
                                }
                            }
                            else if (CompareValue(CurrentJoy.x, -1, -0.5f) && CompareValue(CurrentJoy.y, 0.5f, 1))
                            {
                                if (transform.rotation.eulerAngles.z < 180)
                                {
                                    Switcher = 1;
                                    CurveSide = "L";
                                }
                            }
                            else if (CompareValue(CurrentJoy.x, 0.5f, 1) && CompareValue(CurrentJoy.y, -1, -0.5f))
                            {
                                if (transform.rotation.eulerAngles.z < 180f)
                                {
                                    Switcher = 0;
                                    CurveSide = "R";
                                }
                            }
                            else
                            {
                                Switcher = 4;
                            }

                            if (Switcher < 4)
                            {
                                if (OnSideCurve[Switcher] == true || OnInvertCurve[Switcher] == true)
                                {
                                    Debug.Log("triggered");
                                    OnMoving = true;
                                    EnemyMovement();
                                    StartCoroutine(CurveMov(CurveSide, OnSideCurve[Switcher] == true ? "84" : "85"));
                                    FindMovable();
                                }
                                else if (AbleMovFront[Switcher])
                                {
                                    StartCoroutine(SmoothMov(Switcher, gameObject));

                                    FindMovable();
                                    EnemyMovement();
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        JoyButton.transform.position = Vector3.Lerp(JoyButton.transform.position, JoyOrgpos, Time.deltaTime * 20);
                    }

                }
                else if(!Joytrigger)
                {
                    JoyButton.transform.position = Vector3.Lerp(JoyButton.transform.position, JoyOrgpos, Time.deltaTime * 10);
                }
            }
            #endregion
            #region ｐｃモード
            else if (SaveFunction.SaveAndLoad.EditMode == true)
            {
                if (!OnNormMove && !OnMoving && MapLoader.Instance.startMov && (!TextImage[0].activeSelf && !TextImage[1].activeSelf))
                {
                    FindMovable();

                    if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        if (OnSideCurve[2] == true || OnInvertCurve[2] == true)
                        {
                            Debug.Log("triggered");
                            OnMoving = true;
                            EnemyMovement();
                            StartCoroutine(CurveMov("F", OnSideCurve[2] == true ? "84" : "85"));
                            FindMovable();
                        }
                        else if (AbleMovFront[2])
                        {
                            StartCoroutine(SmoothMov(2, gameObject));

                            FindMovable();
                            EnemyMovement();
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        if (OnSideCurve[3] == true || OnInvertCurve[3] == true)
                        {
                            Debug.Log("triggered");
                            OnMoving = true;
                            EnemyMovement();
                            StartCoroutine(CurveMov("B", OnSideCurve[3] == true ? "84" : "85"));
                            FindMovable();
                        }
                        else if (AbleMovFront[3])
                        {
                            StartCoroutine(SmoothMov(3, gameObject));

                            FindMovable();
                            EnemyMovement();
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (OnSideCurve[1] == true || OnInvertCurve[1] == true)
                        {

                            Debug.Log("triggered");
                            OnMoving = true;
                            EnemyMovement();
                            StartCoroutine(CurveMov("L", OnSideCurve[1] == true ? "84" : "85"));
                            FindMovable();
                        }
                        else if (AbleMovFront[1])
                        {
                            StartCoroutine(SmoothMov(1, gameObject));

                            FindMovable();
                            EnemyMovement();
                        }

                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (OnSideCurve[0] == true || OnInvertCurve[0] == true)
                        {
                            Debug.Log("triggered");
                            OnMoving = true;
                            EnemyMovement();
                            StartCoroutine(CurveMov("R", OnSideCurve[0] == true ? "84" : "85"));
                            FindMovable();
                        }
                        else if (AbleMovFront[0])
                        {
                            StartCoroutine(SmoothMov(0, gameObject));

                            FindMovable();
                            EnemyMovement();
                        }
                    }
                }
            }
            #endregion
        }
        #endregion
        #region マップの全体を見るモード
        else
        {
            if (SaveFunction.SaveAndLoad.EditMode == false)
            {
                Vector2 OrgPos = new Vector2(0, 0);
                if (Joytrigger)
                {
                    Vector3 compareTouch = Vector3.Normalize(Input.mousePosition - JoyButton.transform.position);
                  
                    JoyButton.transform.position = Vector3.Lerp(JoyButton.transform.position, JoyOrgpos + compareTouch * 30, Time.deltaTime * 10);
                    Vector3 CurrentJoy = Vector3.Normalize(JoyButton.transform.position - JoyOrgpos);
                    if (CompareValue(CurrentJoy.x, -0.5f, 0.5f) && CompareValue(CurrentJoy.y, 0.5f, 1f))
                    {
                        Debug.Log("Up");
                        cam.transform.position += new Vector3(0.1f, 0, 0.1f);
                    }
                    else if (CompareValue(CurrentJoy.x, -0.5f, 0.5f) && CompareValue(CurrentJoy.y, -1f, -0.5f))
                    {

                        Debug.Log("Down");
                        cam.transform.position += new Vector3(-0.1f, 0, -0.1f);
                    }
                    else if (CompareValue(CurrentJoy.x, 0.5f, 1) && CompareValue(CurrentJoy.y, 0.5f, 1f))
                    {
                        Debug.Log("Topright");
                        cam.transform.position += new Vector3(0.1f, 0, 0);

                    }
                    else if (CompareValue(CurrentJoy.x, -1, -0.5f) && CompareValue(CurrentJoy.y, -1, -0.5f))
                    {
                        Debug.Log("bottomleft");
                        cam.transform.position += new Vector3(-0.1f, 0, 0);
                    }
                    else if (CompareValue(CurrentJoy.x, -1, -0.5f) && CompareValue(CurrentJoy.y, 0.5f, 1))
                    {
                        Debug.Log("topleft");
                       
                        cam.transform.position += new Vector3(0, 0, 0.1f);
                    }
                    else if (CompareValue(CurrentJoy.x, 0.5f, 1) && CompareValue(CurrentJoy.y, -1, -0.5f))
                    {
                        Debug.Log("btmright");
                        cam.transform.position += new Vector3(0, 0, -0.1f);
                        
                    }
                    else if (CompareValue(CurrentJoy.x, -1, -0.5f) && CompareValue(CurrentJoy.y, -0.5f, 0.5f))
                    {
                        Debug.Log("Left");
                        cam.transform.position += new Vector3(-0.1f, 0, 0.1f);

                    }
                    else if (CompareValue(CurrentJoy.x, 0.5f, 1) && CompareValue(CurrentJoy.y, -0.5f, 0.5f))
                    {
                        Debug.Log("Right");
                        cam.transform.position += new Vector3(0.1f, 0, -0.1f);

                    }
                }
                else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
                {
                         OrgPos = Input.GetTouch(0).position;
                }
                else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
                {
                         Vector2 MovPos = Input.GetTouch(0).deltaPosition;
                         cam.transform.position += Vector3.Normalize(MovPos - OrgPos)*0.5f;
                }
                if (!Joytrigger)
                {
                    JoyButton.transform.position = Vector3.Lerp(JoyButton.transform.position, JoyOrgpos, Time.deltaTime * 10);
                }

            }
                else if(SaveFunction.SaveAndLoad.EditMode == true)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    cam.transform.position += new Vector3(0.1f, 0, 0.1f);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    cam.transform.position += new Vector3(-0.1f, 0, -0.1f);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    cam.transform.position += new Vector3(-0.1f, 0, 0.1f);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    cam.transform.position += new Vector3(0.1f, 0, -0.1f);
                }

            }
        }
        #endregion
    }
    

    #region 敵関連処理
    //敵がダッシュする処理
    IEnumerator EnemyRush()
    {
        enemMoving = true;
        NewMove = false;
        StepReverser = true;
        EnemyStepCounter[0] = 6;
        Vector3 org = SetEnemy[0].transform.position;
        float lerpCounter = 0;
        SetEnemy[0].transform.rotation = Quaternion.Euler(0, -180, 0);
        for (int i = 0; i < 100; i++)
        {
            lerpCounter = (float)i / 100;
            if (i == 99) lerpCounter = 1;
            SetEnemy[0].transform.position = Vector3.Lerp(org, new Vector3(1, 0.5f, 0), lerpCounter);
            yield return new WaitForSeconds(0.01f);
        }
        EnemyNewMove = true;
        enemMoving = false;
        
    }
    //敵を生成する処理
    public void EnemyGenerate()
    {
        for (int i = 0; i < EnemyCount; i++) {
            var EnemyCreated = GameObject.Instantiate(EnemyObj);
            EnemyCreated.transform.name = "Enemy_" + i.ToString();
            EnemyCreated.transform.position = EnemyStartingPos[MapLoader.Instance.StagesNum - 1][i];
            EnemyCreated.transform.rotation *= Quaternion.Euler(EnemyStartingRot[MapLoader.Instance.StagesNum - 1][i]);
            SetEnemy.Add(EnemyCreated);
        }
    }
    //敵の移動
    void EnemyMovement()
    {
        if (!enemMoving && stageNo <3)
        {
            if (EnemyStartMove || stageNo == 2)
            {
                if (EnemyNewMove && stageNo == 0) { EnemyProcedure[stageNo][0][2] = 12; }
                for (int i = 0; i < EnemyCount; i++)
                {
                    
                    switch (EnemyProcedure[stageNo][i][1 + (2 * ProcedureCounter[i])])
                    {
                        case 0:
                            StartCoroutine(SmoothMov(2, SetEnemy[i]));
                            EnemyStepCounter[i]++;
                            if (EnemyStepCounter[i] == EnemyProcedure[stageNo][i][2 + 2 * ProcedureCounter[i]])
                            {
                                EnemyStepCounter[i] = 0;
                                ProcedureCounter[i]++;
                                if (ProcedureCounter[i] == EnemyProcedure[stageNo][i][0]) ProcedureCounter[i] = 0;
                            }
                            break;
                        case 1:
                            SetEnemy[i].transform.rotation *= Quaternion.Euler(0, EnemyProcedure[stageNo][i][2 + 2 * ProcedureCounter[i]], 0);
                            ProcedureCounter[i]++;
                            if (ProcedureCounter[i] == EnemyProcedure[stageNo][i][0]) ProcedureCounter[i] = 0;
                            break;
                    }
                }
            }
        }
       
    }
#endregion

    #region 当たり判定
    //移動後、レーで当たり判定を更新する
    void FindMovable()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 bkd = transform.TransformDirection(-Vector3.forward);
        Vector3 LeftSide = transform.TransformDirection(-Vector3.right);
        Vector3 RightSide = transform.TransformDirection(Vector3.right);

        Debug.DrawRay(transform.position, fwd);
        Debug.DrawRay(transform.position, bkd);
        Debug.DrawRay(transform.position, LeftSide);
        Debug.DrawRay(transform.position, RightSide);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd, out hit, 1))
        {
            if (hit.transform.tag == "84")
            {
               
                if (hit.transform.eulerAngles.y >= 45) { Debug.Log("hit 2"); Upvector[2] = true; }
                OnSideCurve[2] = true;
            }
            else if (hit.transform.tag == "85")
            {
                
                OnInvertCurve[2] = true;
            }
            else
            {
                AbleMovFront[2] = false;
               
                OnSideCurve[2] = false;
                OnInvertCurve[2] = false;
                Upvector[2] = false;
            }
        }
        else
        {
            OnSideCurve[2] = false;
            OnInvertCurve[2] = false;
            Upvector[2] = false;
            AbleMovFront[2] = true;
        }
        if (Physics.Raycast(transform.position, RightSide, out hit, 1))
        {
            if (hit.transform.tag == "84" )
            {
                if (hit.transform.eulerAngles.y >= 45) { Debug.Log("hit 0"); Upvector[0] = true; }
                OnSideCurve[0] = true;
            }
            else if (hit.transform.tag == "85")
            {
                if (hit.transform.gameObject.layer == 8) Upvector[0] = true;
                OnInvertCurve[0] = true;
            }
            else
            {
                AbleMovFront[0] = false;
                OnInvertCurve[0] = false;
                OnSideCurve[0] = false;
                Upvector[0] = false;
            }
        }
        else
        {
            OnInvertCurve[0] = false;
            OnSideCurve[0] = false;
            Upvector[0] = false;
            AbleMovFront[0] = true;
        }
        if (Physics.Raycast(transform.position, LeftSide, out hit, 1))
        {
            if (hit.transform.tag == "84")
            {
                if (hit.transform.eulerAngles.y >= 45) { Debug.Log("hit 1"); Upvector[1] = true; }
                OnSideCurve[1] = true;
            }
            else if (hit.transform.tag == "85")
            {

                OnInvertCurve[1] = true;
            }
            else
            {
                AbleMovFront[1] = false;
                OnInvertCurve[1] = false;
                OnSideCurve[1] = false;
                Upvector[1] = false;
            }
        }
        else
        {
            OnInvertCurve[1] = false;
            OnSideCurve[1] = false;
            Upvector[1] = false;
            AbleMovFront[1] = true;
        }
        if (Physics.Raycast(transform.position, bkd, out hit, 1))
        {
            if (hit.transform.tag == "84" )
            {
                if (hit.transform.eulerAngles.y >= 45) { Debug.Log("hit 3"); Upvector[3] = true; }
                OnSideCurve[3] = true;
            }
            else if (hit.transform.tag == "85" )
            {
                OnInvertCurve[3] = true;
            }
            else
            {
                AbleMovFront[3] = false;
                OnSideCurve[3] = false;
                OnInvertCurve[3] = false;
                Upvector[3] = false;
            }
        }
        else
        {
            AbleMovFront[3] = true;
            OnSideCurve[3] = false;
            OnInvertCurve[3] = false;
            Upvector[3] = false;
        }
    }
    //罠の当たり判定
    private void OnTriggerStay(Collider other)
    {
        if (!OnstayTrigger)
        {
            switch (other.tag)
            {
                case "12":
                    if (other.gameObject.layer == 11 && MapLoader.Instance.FlashSwitchOne == false)
                    {
                        Death = true;
                        OnstayTrigger = true;
                        ResetSpawn();
                    }
                    if (other.gameObject.layer == 12 && MapLoader.Instance.FlashSwitchTwo == false)
                    {
                        Death = true;
                        OnstayTrigger = true;
                        ResetSpawn();
                    }
                    break;
                default:
                    break;
            }
        }
    }
    //様々な特殊キューブの当たり判定処理
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "11":
                StartCoroutine(MoveSwitch(other));
                SwitchTrigger(other.name,other);
                break;
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
                break;
            case "87":
                Explode(other.gameObject);
                Debug.Log("Explode");
                break;
            case "88":
                StartTeleport = true;
                TeleporterName = other.transform.name;
                break;
            case "89":
                if(other.gameObject.layer == 11)
                {
                    EnemyStartMove = true;
                }else if(other.gameObject.layer == 12)
                {
                    Debug.Log("12box");
                    if (!EnemyNewMove)
                    {
                        NewMove = true;
                    }
                }
                break;
            case "Enemy":
                GameOver(0);
                break;
            case "12":
                if (other.gameObject.layer == 11 && MapLoader.Instance.FlashSwitchOne == false)
                {
                    Death = true;
                    OnstayTrigger = true;
                }
                if(other.gameObject.layer==12 && MapLoader.Instance.FlashSwitchTwo == false)
                {
                    Death = true;
                    OnstayTrigger = true;
                }
                break;
            case "9":
                GameOver(1);
                break;
            default:
                break;
        }
    }
    //火の当たり判定
    void OnParticleCollision(GameObject other)
    {
        GameOver(0);
    }
    #endregion

    #region ギミック
    //踏んだswitchを地面に沈む処理
    IEnumerator MoveSwitch(Collider other)
    {
        for (int i = 0; i < 10; i++)
        {
            if (other.gameObject.layer == 0)
            {
                other.transform.position += new Vector3(0, 0, 0.01f);
            }
            else
            {
                other.transform.position += new Vector3(0, -0.01f, 0);
            }
            yield return new WaitForSeconds(0.05f);
        }
        var TempName = other.gameObject.name.Split('_');
        if(MapLoader.Instance.Rewind && TempName[1] == "0")
        GetComponent<AudioSource>().Play();
    }
    //踏んだら橋を作る処理
   private void SwitchTrigger(string SwitchName,Collider other)
    {
        string[] SplitName = SwitchName.Split('_');
            if (SplitName[1] == "0")
            {
                MapLoader.Instance.CameraPlaceOne = true;
                if(other.gameObject.layer == 11)
                {
                MapLoader.Instance.Rewind = true;
                }
            }
            else if (SplitName[1] == "1")
            {
                MapLoader.Instance.CameraPlaceTwo = true;
                if(other.gameObject.layer == 0)
                    DeathCount = 0;
            }
        
    }
    //転移処理
    private void Teleport(string ColliderName)
    {
        Debug.Log("teleport");
        string[] SplitName = ColliderName.Split('_');
        int test = -1;
        int OutPos = 0;
        if(int.TryParse(SplitName[1], out test))
        { 
            OutPos = test;
        }
        else
        {
            OutPos = 0;
        }
        transform.position = SaveFunction.SaveAndLoad.TeleportTarget[OutPos];
        FindMovable();
        StartTeleport = false;
    }
    //爆発処理
    private void Explode(GameObject other)
    {
        if (other.gameObject.layer == 12) { explodecontrol = true; }
        if (other.layer == 11)
        {
            EnemyStartMove = true;
            Debug.Log("layer 11");
            Transform MapObj = GameObject.Find("ObjectTempPos").transform;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 6.0f);
            foreach (Collider hit in colliders)
            {
                if (hit.gameObject.GetComponent<Rigidbody>() == null && hit.gameObject.layer != 13)
                {
                    hit.gameObject.AddComponent<Rigidbody>();
                    hit.gameObject.layer = 11;
                }
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(200.0f, transform.position, 6.0f, 3.0F);
            }
        }
        Prb.useGravity = true;
        Prb.isKinematic = false;
        MapLoader.Instance.startMov = false;
        MapLoader.Instance.CameraPlaceThree = true;
        transform.rotation = Quaternion.Euler(0, 90, 0);
        Prb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        
    }
    #endregion

    #region 移動処理
    //滑らかの移動処理
    IEnumerator SmoothMov(int Direction,GameObject WhatToMov)
    {
        
        float a = 0;
        OnNormMove = true;
        yield return null;
        Vector3 NextPos;
        Vector3 OrgPos = WhatToMov.transform.position;
        Quaternion OrgRot = WhatToMov.transform.rotation;
        switch (Direction)
        {
            case 0:
                NextPos = WhatToMov.transform.position + WhatToMov.transform.right;
                
                break;
            case 1:
                NextPos = WhatToMov.transform.position - WhatToMov.transform.right;
                
                break;
            case 2:
                NextPos = WhatToMov.transform.position + WhatToMov.transform.forward;
                
                break;
            case 3:
                NextPos = WhatToMov.transform.position - WhatToMov.transform.forward;
                
                break;
            default:
                NextPos = new Vector3(0, 0, 0);
                Debug.Log("error");
                break;
        }
        for(int i = 0; i < 10; i++)
        {
            a += 0.1f;
            if (i == 9) a = 1;
            
            WhatToMov.transform.position = Vector3.Lerp(OrgPos, NextPos, a);
            yield return new WaitForSeconds(0.01f);
        }
        if (NewMove && !enemMoving) EnemyNewMove = true;
        FindMovable();
        OnNormMove = false;
        if(StartTeleport == true)
        {
            Teleport(TeleporterName);
        }
        if(Death)ResetSpawn();
    }
    //曲がる移動処理
    IEnumerator CurveMov(string RotateSide,string Tags)
    {
        Debug.Log("ran");
        Vector3 SideRotPoint;
        Vector3 RotAxis = Vector3.zero;
        int RotInvert = 0;
        switch (Tags) {
            case "84":
                Debug.Log("84");
                SideRotPoint = transform.position - (transform.up * 2.2f);
                RotInvert = 1;
                break;
            case "85":
                SideRotPoint = transform.position + (transform.up*0.8f);
                RotInvert = -1;
                break;
            default:
                SideRotPoint = new Vector3(0, 0, 0);
                Debug.Log("CurvMov Error");
                break;
        }

        
        float RotAngle;
        switch (RotateSide)
        {
            case "L":
                RotAngle = 2 * RotInvert;
                break;
            case "R":
                RotAngle = -2 * RotInvert;
                break;
            case "F":
                RotAngle = -2 * RotInvert;
                break;
            case "B":
                RotAngle = 2 * RotInvert;
                break;
            default:
                RotAngle = 0;
                Debug.Log("got problem");
                break;
        }
        if (RotateSide == "L" || RotateSide == "R")
        {
            if (Upvector[0] || Upvector[1] || Upvector[2] || Upvector[3]) RotAxis = Vector3.up;
            else RotAxis = Vector3.right;
            for (int i = 0; i < 45; i++)
            {
                transform.RotateAround(SideRotPoint, RotAxis, RotAngle);
                yield return new WaitForSeconds(RotInvert>0?0.01f:0.005f);
            }
        }
        else if(RotateSide == "F" || RotateSide == "B")
        {
            if (Upvector[0] || Upvector[1]||Upvector[2]||Upvector[3]) RotAxis = Vector3.up;
            else RotAxis = Vector3.forward;
            for (int i = 0; i < 45; i++)
            {
                transform.RotateAround(SideRotPoint,RotAxis, RotAngle);
                yield return new WaitForSeconds(RotInvert > 0 ? 0.01f : 0.005f);
            }
        }
        OnMoving = false;
        if (StartTeleport == true)
        {
            Teleport(TeleporterName);
        }
    }
    #endregion

    #region ゲームシステム関連
    void GameOver(int ImageIndex)
    {
        MapLoader.Instance._gameOver = true;
        StartCoroutine(_LoadGameOver(ImageIndex));
    }
  
    void ResetSpawn()
    {
        Debug.Log("respawn");
        if (Death)
        {
            bool BackPlace = true;

            if (DeathCount == 0)
            {
                GameOver(0);
                BackPlace = false;
            }

            if (BackPlace)
            {
                DeathCount--;
                Debug.Log(DeathCount);
                gameObject.transform.position = new Vector3(14, -14, 7);
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

            }

            Death = false;
            OnstayTrigger = false;

        }
    }
    IEnumerator _LoadGameOver(int _imageIndex)
    {
        yield return new WaitForSeconds(DeadCamTime);
        TextImage[_imageIndex].SetActive(true);

    }
    #endregion

    #region その他
    public void onPress()
    {
        Joytrigger = true;
    }
    public void onRelease()
    {
        Joytrigger = false;
    }
    private bool CompareValue(float CompareNum, float min, float max)
    {
        if (CompareNum > min && CompareNum < max)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
