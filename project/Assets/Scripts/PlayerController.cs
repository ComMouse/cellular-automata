using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public int id;

    [HideInInspector]
    public float speedRatio;

    private bool hasItem;

    private bool hasKid;

    [HideInInspector]
    public bool isInBed;

    private int fightResult;

    [HideInInspector]
    public bool isAlive;

    private bool lastPress;

    private float lastTime;

	// Use this for initialization
	void Start () {
        hasItem = false;
        hasKid = false;
        isInBed = false;
        isAlive = true;
        lastPress = false;
        fightResult = 0;
        lastTime = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (hasItem && gameObject.GetComponent<Move>().curCoord.x > 0 && gameObject.GetComponent<Move>().curCoord.x < 12 && gameObject.GetComponent<Move>().curCoord.y > 0 && gameObject.GetComponent<Move>().curCoord.y < 9)
            {
                GameManager.instance.KidWin();
            }
            if(hasKid && gameObject.GetComponent<Move>().curCoord.x > 17 && gameObject.GetComponent<Move>().curCoord.x <28 && gameObject.GetComponent<Move>().curCoord.y > 26 && gameObject.GetComponent<Move>().curCoord.y < 37)
            {
                hasKid = false;
                PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
                for (int i = 0; i < players.Length; i++)
                {
                    if (!players[i].isAlive)
                    {
                        players[i].isInBed = true;
                        players[i].transform.position = LevelData.instance.Coord2WorldPos(new LevelCoord(21, 35));
                        players[i].transform.parent = null;
                    }
                }
            }
            if(lastTime >= 0 && !lastPress && (gameObject.GetComponent<Move>().inputCtrl.GetButton1Down() || gameObject.GetComponent<Move>().inputCtrl.GetButton2Down()) && id == 3)
            {
                fightResult ++;
            }
        }
        else
        {
            if (!isInBed && !lastPress && (gameObject.GetComponent<Move>().inputCtrl.GetButton1Down() || gameObject.GetComponent<Move>().inputCtrl.GetButton2Down()))
            {
                PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].id == 3)
                    {
                        players[i].fightResult --;
                    }
                }
            }
        }
        if(Time.time-lastTime >= GameManager.instance.fightTime)
        {
            if(fightResult < 0)
            {
                speedRatio = 0.25f;
            }
            else
            {
                speedRatio = 1.0f;
            }
            fightResult = 0;
            lastTime = Time.time;
        }
        lastPress = gameObject.GetComponent<Move>().inputCtrl.GetButton1Down() || gameObject.GetComponent<Move>().inputCtrl.GetButton2Down();
    }

    public void PickupItem(LevelCoord coord, int itemid)
    {
        if (!hasItem)
        {
            if (itemid == (int)GridType.LootSpawn1)
            {
                if(id == 3)
                    GameManager.instance.MomWin();
                hasItem = true;

                SoundManager.Instance.Play("Effect_GetController");
            }
            else
            {
                SoundManager.Instance.Play("Effect_CatchByMom");
            }
            LootItemManager.instance.ItemPickup(coord, itemid - (int)GridType.LootSpawn1);
            //LevelData.instance.OriginMap[coord.y, coord.x] = -1;
        }
    }

    public void KnockoutPlayer(int playerid)
    {
        PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
        for(int i = 0; i < players.Length; i++)
        {
            if(players[i].id == playerid)
            {
                //if (players[i].hasItem)
                //    GameManager.instance.MomWin();
                if(players[i].isAlive)
                {
                    hasKid = true;
                    players[i].isAlive = false;
                    players[i].transform.position = transform.position;
                    players[i].transform.parent = transform;
                    players[i].gameObject.GetComponent<Move>().StopMoving();
                    GameManager.instance.kidAlive--;
                    if (lastTime < 0)
                        lastTime = Time.time;

                    SoundManager.Instance.Play("Effect_BabyCry");
                }
            }
        }
    }
}
