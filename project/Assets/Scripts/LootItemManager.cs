using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItemManager : MonoBehaviour {

    public static LootItemManager instance { get; private set; }

    [SerializeField]
    private int[] itemNums = new int[8];

    [SerializeField]
    private GameObject[] itemPrefabs = new GameObject[8];

    private List<GameObject> itemPool; 

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    public void StartGame () {
		for(int i = 0; i < 8; i++)
        {
            List<LevelCoord> tmpset = LevelData.instance.GetPlayerSpawnPoint(i + 1);
            if (tmpset.Count < itemNums[i])
            {
                Debug.Log("Error: Uncompatible count for id: " + i + 1);
                break;
            }
            for (int j = itemNums[i]; j < tmpset.Count; j++)
            {
                int k = Random.Range(0, j - 1);
                if (k < itemNums[i])
                {
                    LevelCoord tmpCoord = tmpset[j];
                    tmpset[j] = tmpset[k];
                    tmpset[k] = tmpCoord;
                }
            }
            for(int j = 0; j < tmpset.Count; j++)
            {
                if (j < itemNums[i])
                {
                    var tmpPrefab = Instantiate(itemPrefabs[i]);
                    tmpPrefab.transform.position = LevelData.instance.Coord2WorldPos(tmpset[j]);
                    itemPool.Add(tmpPrefab);
                }
                else
                {
                    LevelData.instance.OriginMap[tmpset[j].y, tmpset[j].x] = -1;
                    LevelData.instance.GridMap[tmpset[j].y, tmpset[j].x] = -1;
                }
            }
        }
	}
	
    public void ItemPickup(LevelCoord coord)
    {
        for(int i = 0; i < itemPool.Count; i++)
        {
            if(LevelData.instance.WorldPos2Coord(itemPool[i].transform.position) == coord)
            {
                Destroy(itemPool[i]);
                itemPool.RemoveAt(i);
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
