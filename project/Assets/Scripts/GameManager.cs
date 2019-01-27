using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance { get; private set; }

    public float ticktime;

    public float speed;

    public float fightTime;

    private bool gameSet = false;

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
        if (!gameSet)
        {
            Debug.Log("Mom Win!");
            gameSet = true;
            SoundManager.Instance.Play("Effect_FailureEnding");
            SoundManager.Instance.FadeTo("Music_MemoCute", 0.1f, 0.5f);
        }
    }

    public void KidWin()
    {
        if (!gameSet)
        {
            Debug.Log("Kids Win!");
            gameSet = true;

            SoundManager.Instance.Play("Effect_FailureEnding");
            SoundManager.Instance.FadeTo("Music_MemoCute", 0.1f, 0.5f);
        }
    }
}
