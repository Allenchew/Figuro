using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//崩壊した橋の時間を巻き戻しのエフェクトの処理
public class ReverseBridge : MonoBehaviour {
    private bool ran = false;
    private Rigidbody rb;

    private List<Vector3[]> RecordPos = new List<Vector3[]>();
	// Use this for initialization
	void Start () {
        float RandomFall = Random.Range(-0.5f, 0.5f);
        gameObject.AddComponent<Rigidbody>();
        rb = gameObject.GetComponent<Rigidbody>();
        RecordPos.Add(new Vector3[] { gameObject.transform.position, gameObject.transform.rotation.eulerAngles });
        rb.AddExplosionForce(1000.0f, transform.position+ new Vector3(0,0,RandomFall), 2.0f, -1.0F);
        gameObject.GetComponent<Collider>().isTrigger = true;
        Debug.Log("reloaded");
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.transform.position.y > -16)
        {
            RecordPos.Add(new Vector3[] { gameObject.transform.position, gameObject.transform.rotation.eulerAngles});
        }else
        {
            Destroy(rb);
        }
        if(!ran && MapLoader.Instance.Rewind)
        {
            ran = true;
            StartCoroutine("RewindBridge");
        }
	}

    IEnumerator RewindBridge()
    {
        gameObject.GetComponent<Collider>().isTrigger = true;
        yield return new WaitForSeconds(2.0f);
        for(int i= RecordPos.Count-1; i >= 0; i--)
        {
            gameObject.transform.position = RecordPos[i][0];
            gameObject.transform.rotation = Quaternion.Euler(RecordPos[i][1]);
            yield return new WaitForSeconds(0.09f);
        }

    }
}
