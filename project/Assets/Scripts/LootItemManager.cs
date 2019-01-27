using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItemManager : MonoBehaviour {

    public static LootItemManager instance { get; private set; }

    //[SerializeField]
    //private int[] itemNums = new int[8];

    [SerializeField]
    private int itemNums;

    [SerializeField]
    private GameObject[] itemPrefabs = new GameObject[8];

    [SerializeField]
    private GameObject closetPrefab;

    private List<GameObject> itemPool = new List<GameObject>(); 

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    //   public void StartGame () {
    //	for(int i = 0; i < 8; i++)
    //       {
    //           List<LevelCoord> tmpset = LevelData.instance.GetLootSpawnPoint(i);
    //           if (tmpset.Count < itemNums[i])
    //           {
    //               Debug.Log("Error: Uncompatible count for id: " + i + 1);
    //               break;
    //           }
    //           for (int j = itemNums[i]; j < tmpset.Count; j++)
    //           {
    //               int k = Random.Range(0, j - 1);
    //               if (k < itemNums[i])
    //               {
    //                   LevelCoord tmpCoord = tmpset[j];
    //                   tmpset[j] = tmpset[k];
    //                   tmpset[k] = tmpCoord;
    //               }
    //           }
    //           for(int j = 0; j < tmpset.Count; j++)
    //           {
    //               if (j < itemNums[i])
    //               {
    //                   var tmpPrefab = Instantiate(itemPrefabs[i]);
    //                   tmpPrefab.transform.position = LevelData.instance.Coord2WorldPos(tmpset[j]);
    //                   itemPool.Add(tmpPrefab);
    //                   LevelData.instance.OriginMap[tmpset[j].y, tmpset[j].x] = 1;
    //                   LevelData.instance.GridMap[tmpset[j].y, tmpset[j].x] = 1;
    //               }
    //               else
    //               {
    //                   var tmpPrefab = Instantiate(closetPrefab);
    //                   tmpPrefab.transform.position = LevelData.instance.Coord2WorldPos(tmpset[j]);
    //                   itemPool.Add(tmpPrefab);
    //                   LevelData.instance.OriginMap[tmpset[j].y, tmpset[j].x] = -1;
    //                   LevelData.instance.GridMap[tmpset[j].y, tmpset[j].x] = -1;
    //               }
    //           }
    //       }
    //}

    public void StartGame()
    {
        List<LevelCoord> tmpset = LevelData.instance.GetLootSpawnPoint(0);
        if (tmpset.Count < itemNums)
        {
            Debug.Log("Error: Uncompatible count");
            return;
        }
        for (int i = itemNums; i < tmpset.Count; i++)
        {
            int j = Random.Range(0, i - 1);
            if (j < itemNums)
            {
                LevelCoord tmpCoord = tmpset[i];
                tmpset[i] = tmpset[j];
                tmpset[j] = tmpCoord;
            }
        }
        for (int i = 0; i < tmpset.Count; i++)
        {
            var tmpPrefab = Instantiate(closetPrefab);
            tmpPrefab.transform.position = LevelData.instance.Coord2WorldPos(tmpset[i]);
            itemPool.Add(tmpPrefab);
            if (i >= itemNums){
                int tmpSurprise = Random.Range((int)GridType.LootSpawn2, (int)GridType.LootSpawn8);
                LevelData.instance.OriginMap[tmpset[i].y, tmpset[i].x] = tmpSurprise;
                LevelData.instance.GridMap[tmpset[i].y, tmpset[i].x] = tmpSurprise;
            }
        }
    }

    public void ItemPickup(LevelCoord coord, int id)
    {
        for (int i = 0; i < itemPool.Count; i++)
        {
            if (LevelData.instance.WorldPos2Coord(itemPool[i].transform.position) == coord)
            {
                Destroy(itemPool[i]);
                itemPool.RemoveAt(i);
                var tmpPrefab = Instantiate(itemPrefabs[id]);
                tmpPrefab.transform.position = LevelData.instance.Coord2WorldPos(coord);
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
