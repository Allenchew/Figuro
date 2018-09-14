using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGimmic : MonoBehaviour {
    
    private bool MovedIn = false;
    private bool MovedOut = false;
    private bool Movingtrap = false;
    private bool initialtrigger = false;
    public Material TrapB;
    // Use this for initialization
    void Start () {
        if(gameObject.layer == 12)
        {
            gameObject.GetComponent<Renderer>().material = TrapB;
        }
    }

	// Update is called once per frame
	void Update () {
		if(MapLoader.Instance.SwitchOne == true )
        {
            if(gameObject.layer == 11 && Movingtrap == false)
            {
                Movingtrap = true;
                float movingDirection;

                if (MovedIn == false)  movingDirection = 0.01f;
                else movingDirection = -0.01f; 
                MovedIn = !MovedIn;
                StartCoroutine(MovingTrap(movingDirection,3f));
            }
            if (gameObject.layer == 12 && Movingtrap == false )
            {
                Movingtrap = true;
                float movingDirection;

                if (MovedOut == false) movingDirection = -0.01f; 
                else  movingDirection = 0.01f; 
               
                MovedOut = !MovedOut;
                StartCoroutine(MovingTrap(movingDirection, 3f));
                //}
                
            }
        }
	}
    IEnumerator MovingTrap(float MoveSide,float delaytime)
    {
        /* if (gameObject.layer == 11 && !MovedIn)
             yield return new WaitForSeconds(2f);*/
        
        yield return new WaitForSeconds(delaytime-1);
        StartCoroutine("FlashTrap");
        yield return new WaitForSeconds(1f);
        if (!MovedIn && gameObject.layer == 11) MapLoader.Instance.FlashSwitchOne = true;
        if (MovedOut && gameObject.layer == 12) MapLoader.Instance.FlashSwitchTwo = true;
        

        for(int i = 0; i < 10; i++)
        {
            gameObject.transform.position += new Vector3(0, 0, MoveSide);
            yield return new WaitForSeconds(0.05f);
        }
        if (!MovedIn && gameObject.layer == 11) MapLoader.Instance.FlashSwitchOne = false;
        if (MovedOut && gameObject.layer == 12) MapLoader.Instance.FlashSwitchTwo = false;
        Movingtrap = false;
        
    }
    IEnumerator FlashTrap()
    {
        Color ChangingColor;
        bool ChangedColor = false;

        if ((gameObject.layer == 11 && MovedIn) || (gameObject.layer == 12 && !MovedOut))
        {
            for (int j = 0; j < 6; j++)
            {
                if (!ChangedColor)
                    ColorUtility.TryParseHtmlString("#440000", out ChangingColor);
                else
                    ColorUtility.TryParseHtmlString("#FF0000", out ChangingColor);
                gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", ChangingColor);
                ChangedColor = !ChangedColor;
                yield return new WaitForSeconds(0.167f);
            }
            
        }


    }
}
