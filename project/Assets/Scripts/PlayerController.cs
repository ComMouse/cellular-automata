using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private bool hasItem;

	// Use this for initialization
	void Start () {
        hasItem = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PickupItem(LevelCoord coord)
    {
        if (!hasItem)
        {
            hasItem = true;
            int grid = LevelData.instance.GridMap[coord.y, coord.x] - (int)GridType.LootSpawn1 + 1;
            LootItemManager.instance.ItemPickup(coord);
            LevelData.instance.OriginMap[coord.y, coord.x] = -1;
        }
    }
}
