using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTest : MonoBehaviour
{
    public LevelData level;
    public string levelPath;

	// Use this for initialization
	void Start () {
        Debug.Assert(level != null);

		LevelLoader loader = new LevelLoader();
        loader.Load(levelPath);

        level.SetGridMap(loader.GetMap());
        level.Generate();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
