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

    public void PickupItem(LevelCoord coord, int grid)
    {
        if (!hasItem)
        {
            if (grid == (int)GridType.LootSpawn1)
                hasItem = true;
            LootItemManager.instance.ItemPickup(coord, grid - (int)GridType.LootSpawn1);
            //LevelData.instance.OriginMap[coord.y, coord.x] = -1;
        }
    }
}
