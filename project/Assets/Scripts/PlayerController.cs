using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    public int id;

    private bool hasItem;

    [HideInInspector]
    public bool isAlive;

	// Use this for initialization
	void Start () {
        hasItem = false;
        isAlive = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (hasItem && isAlive && gameObject.GetComponent<Move>().curCoord.x > 0 && gameObject.GetComponent<Move>().curCoord.x < 12 && gameObject.GetComponent<Move>().curCoord.y > 0 && gameObject.GetComponent<Move>().curCoord.y < 9)
        {
            GameManager.instance.KidWin();
        }
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
                    players[i].isAlive = false;
                    players[i].gameObject.GetComponent<Move>().StopMoving();
                    GameManager.instance.kidAlive--;
                }
            }
        }
    }
}
