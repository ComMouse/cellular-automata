using System.Collections;
using System.Collections.Generic;
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

    private LevelCoord targetCoord;

    private LevelCoord curCoord;

	// Use this for initialization
	void Start () {
        lastTime = 0;
        curCoord = LevelData.instance.WorldPos2Coord(transform.position);
        targetCoord = curCoord;
    }
	
	// Update is called once per frame
	void Update () {
        if (dir == Direction.Stay)
        {
            dir = Mathf.Abs(GetInputX()) > Mathf.Abs(GetInputY()) ? (GetInputX() > 0 ? Direction.Right : Direction.Left) : (GetInputY() > 0 ? Direction.Up : Direction.Down);
        }
        lastTime += Time.deltaTime;
        if(lastTime > ticktime)
        {
            lastTime -= ticktime;
            Tick();
        }
        curCoord = LevelData.instance.WorldPos2Coord(transform.position);
        transform.position = Vector3.MoveTowards(transform.position, LevelData.instance.Coord2WorldPos(targetCoord), speed * Time.deltaTime);
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
            return;

        LevelCoord nextCoord = GetNextCoord();
        if (LevelData.instance.GridMap[nextCoord.x, nextCoord.y] == (int)GridType.Wall)
        {
            targetCoord = curCoord;
            dir = Direction.Stay;
        }
        else
            targetCoord = nextCoord;
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
}
