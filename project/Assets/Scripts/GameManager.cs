using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance { get; private set; }

    public int kidAlive;

    // Use this for initialization
    void Awake () {
        instance = this;
    }
	
	// Update is called once per frame
	void Update () {
		if(kidAlive <= 0)
        {
            MomWin();
        }
	}

    public void MomWin()
    {
        Debug.Log("Mom Win!");
    }

    public void KidWin()
    {
        Debug.Log("Kids Win!");
    }
}
