﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Move : MonoBehaviour {

    private enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        Stay
    }

    [SerializeField]
    private float ticktime;

    [SerializeField]
    private float speed;
    
    private int id;

    [SerializeField]
    private InputController inputCtrl;

    private Direction dir;

    private float lastTime;

    public LevelCoord targetCoord;

    public LevelCoord curCoord;

    private bool isStart;

	// Use this for initialization
	void StartGame () {
        if (inputCtrl == null)
        {
            inputCtrl = gameObject.AddComponent<InputController>();
        }
        id = gameObject.GetComponent<PlayerController>().id;
        lastTime = 0;
        dir = Direction.Stay;
        curCoord = LevelData.instance.GetPlayerSpawnPoint(id).First();
        transform.position = LevelData.instance.Coord2WorldPos(curCoord);
        LevelData.instance.GridMap[curCoord.y, curCoord.x] = -id - 2;
        //LevelData.instance.OriginMap[curCoord.y, curCoord.x] = -1;
        targetCoord = curCoord;
        isStart = true;
        Debug.Log("CurrentCoord:" + curCoord.x + " , " + curCoord.y);
        Debug.Log("TargetCoord:" + targetCoord.x + " , " + targetCoord.y );
    }
	
	// Update is called once per frame
	void Update () {
        if (isStart && gameObject.GetComponent<PlayerController>().isAlive)
        {
            if (dir == Direction.Stay)
            {
                Vector2 inputAxis = new Vector2(GetInputX(), GetInputY());
                if (inputAxis != Vector2.zero)
                    dir = Mathf.Abs(GetInputX()) > Mathf.Abs(GetInputY()) ? (GetInputX() > 0 ? Direction.Right : Direction.Left) : (GetInputY() > 0 ? Direction.Up : Direction.Down);
            }
            lastTime += Time.deltaTime;
            if (lastTime > ticktime)
            {
                lastTime -= ticktime;
                Tick();
            }
            curCoord = LevelData.instance.WorldPos2Coord(transform.position);
            transform.position = Vector3.MoveTowards(transform.position, LevelData.instance.Coord2WorldPos(targetCoord), speed * Time.deltaTime);
        }
        else
        {
            if (LevelData.instance.isGenerated && !isStart)
                StartGame();
        }
    }

    private float GetInputX()
    {
        return inputCtrl.GetAxis().x;
    }

    private float GetInputY()
    {
        return inputCtrl.GetAxis().y;
    }

    private void Tick()
    {
        if (dir == Direction.Stay)
        {
            return;
        }
        int grid = LevelData.instance.GridMap[curCoord.y, curCoord.x];
        if (!(LevelData.instance.IsEmpty(grid) || (LevelData.instance.IsOccupiedByPlayer(grid) && grid == -id - 2)))
        {
            Debug.Log(LevelData.instance.GridMap[curCoord.y, curCoord.x]);
            targetCoord = GetLastCoord();
            LevelData.instance.GridMap[targetCoord.y, targetCoord.x] = -id - 2;
            dir = Direction.Stay;
        }
        else
        {
            LevelCoord nextCoord = GetNextCoord();
            grid = LevelData.instance.GridMap[nextCoord.y, nextCoord.x];
            if (!(LevelData.instance.IsEmpty(grid) || (LevelData.instance.IsOccupiedByPlayer(grid) && grid == -id - 2)))
            {
                if (LevelData.instance.IsLootItem(grid))
                {
                    gameObject.GetComponent<PlayerController>().PickupItem(nextCoord, grid);
                }
                else if(id == 3 && LevelData.instance.IsOccupiedByPlayer(grid))
                {
                    gameObject.GetComponent<PlayerController>().KnockoutPlayer(-grid - 2);
                }
                targetCoord = curCoord;
                dir = Direction.Stay;
            }
            else
            {
                targetCoord = nextCoord;
                //if (LevelData.instance.IsLootItem(grid))
                //{
                //    gameObject.GetComponent<PlayerController>().PickupItem(targetCoord);
                //}
                LevelData.instance.GridMap[targetCoord.y, targetCoord.x] = -id - 2;
                LevelData.instance.GridMap[curCoord.y, curCoord.x] = LevelData.instance.OriginMap[curCoord.y, curCoord.x];
            }
            //if (dir == Direction.Stay)
            //    LevelData.instance.GridMap[curCoord.y, curCoord.x] = -id - 2;
            //else
            //    LevelData.instance.GridMap[curCoord.y, curCoord.x] = -1;
        }
    }

    private LevelCoord GetNextCoord()
    {
        switch (dir)
        {
            case Direction.Left:
                return new LevelCoord(curCoord.x - 1, curCoord.y);
            case Direction.Right:
                return new LevelCoord(curCoord.x + 1, curCoord.y);
            case Direction.Up:
                return new LevelCoord(curCoord.x, curCoord.y + 1);
            case Direction.Down:
                return new LevelCoord(curCoord.x, curCoord.y - 1);
            default:
                break;
        }
        return curCoord;
    }

    private LevelCoord GetLastCoord()
    {
        LevelCoord lastCoord = curCoord;
        switch (dir)
        {
            case Direction.Left:
                lastCoord = new LevelCoord(curCoord.x + 1, curCoord.y);
                break;
            case Direction.Right:
                lastCoord = new LevelCoord(curCoord.x - 1, curCoord.y);
                break;
            case Direction.Up:
                lastCoord = new LevelCoord(curCoord.x, curCoord.y - 1);
                break;
            case Direction.Down:
                lastCoord = new LevelCoord(curCoord.x, curCoord.y + 1);
                break;
            default:
                break;
        }
        int grid = LevelData.instance.GridMap[lastCoord.y, lastCoord.x];
        return (LevelData.instance.IsEmpty(grid) || (LevelData.instance.IsOccupiedByPlayer(grid) && grid == -id - 2)) ? lastCoord : curCoord;
    }

    public void StopMoving()
    {
        LevelData.instance.GridMap[curCoord.y, curCoord.x] = LevelData.instance.OriginMap[curCoord.y, curCoord.x];
        targetCoord = curCoord = LevelData.instance.GetPlayerSpawnPoint(id).First();
        LevelData.instance.GridMap[targetCoord.y, targetCoord.x] = -id - 2;
        transform.position = LevelData.instance.Coord2WorldPos(curCoord);
    }

}
