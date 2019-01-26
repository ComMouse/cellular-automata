using System.Collections;
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

    [SerializeField]
    private int id;

    private Direction dir;

    private float lastTime;

    public LevelCoord targetCoord;

    public LevelCoord curCoord;

    private bool isStart;

    private bool isBacking;

	// Use this for initialization
	void StartGame () {
        lastTime = 0;
        dir = Direction.Stay;
        curCoord = LevelData.instance.GetPlayerSpawnPoint(id).First();
        transform.position = LevelData.instance.Coord2WorldPos(curCoord);
        LevelData.instance.GridMap[curCoord.y, curCoord.x] = -id - 1;
        LevelData.instance.OriginMap[curCoord.y, curCoord.x] = -1;
        targetCoord = curCoord;
        isStart = true;
        Debug.Log("CurrentCoord:" + curCoord.x + " , " + curCoord.y);
        Debug.Log("TargetCoord:" + targetCoord.x + " , " + targetCoord.y );
    }
	
	// Update is called once per frame
	void Update () {
        if (isStart)
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
            if (LevelData.instance.isGenerated)
                StartGame();
        }
    }

    private float GetInputX()
    {
        return Input.GetAxis("Horizontal " + id.ToString());
    }

    private float GetInputY()
    {
        return Input.GetAxis("Vertical " + id.ToString());
    }

    private void Tick()
    {
        if (dir == Direction.Stay)
        {
            return;
        }
        int grid = LevelData.instance.GridMap[curCoord.y, curCoord.x];
        if (!(LevelData.instance.IsEmpty(grid) || (LevelData.instance.IsOccupiedByPlayer(grid) && grid == -id - 1)))
        {
            Debug.Log(LevelData.instance.GridMap[curCoord.y, curCoord.x]);
            targetCoord = GetLastCoord();
            LevelData.instance.GridMap[targetCoord.y, targetCoord.x] = -id - 1;
            dir = Direction.Stay;
        }
        else
        {
            LevelCoord nextCoord = GetNextCoord();
            grid = LevelData.instance.GridMap[nextCoord.y, nextCoord.x];
            if (!(LevelData.instance.IsEmpty(grid) || (LevelData.instance.IsOccupiedByPlayer(grid) && grid == -id - 1)))
            {
                targetCoord = curCoord;
                dir = Direction.Stay;
            }
            else
            {
                targetCoord = nextCoord;
                if (LevelData.instance.IsLootItem(grid))
                {
                    gameObject.GetComponent<PlayerController>().PickupItem(targetCoord);
                }
                LevelData.instance.GridMap[targetCoord.y, targetCoord.x] = -id - 1;
                LevelData.instance.GridMap[curCoord.y, curCoord.x] = LevelData.instance.OriginMap[curCoord.y, curCoord.x];
            }
            //if (dir == Direction.Stay)
            //    LevelData.instance.GridMap[curCoord.y, curCoord.x] = -id - 1;
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
        return (LevelData.instance.IsEmpty(grid) || (LevelData.instance.IsOccupiedByPlayer(grid) && grid == -id - 1)) ? lastCoord : curCoord;
    }

}
