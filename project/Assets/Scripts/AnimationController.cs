using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private Move moveCtrl;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private SpriteRenderer sprite;

    private Move.Direction lastDir = Move.Direction.Down;
    private bool lastIsMoving = false;

    private void Start()
    {
        Debug.Assert(animator != null);
        Debug.Assert(sprite != null);

        PlayState(AnimationDir.Down, false);
    }

    private void Update()
    {
        if (moveCtrl == null)
            return;

        sprite.sortingOrder = 10 + LevelData.instance.Height - moveCtrl.curCoord.y;

        var currentDir = moveCtrl.Dir;
        var currentIsMoving = moveCtrl.Dir != Move.Direction.Stay;

        if (!currentIsMoving)
        {
            currentDir = lastDir;
        }

        if (currentIsMoving != lastIsMoving || lastDir != currentDir)
        {
            PlayState(MoveDir2AnimDir(currentDir), currentIsMoving);
        }

        lastDir = currentDir;
        lastIsMoving = currentIsMoving;
    }

    private void PlayState(AnimationDir dir, bool isMoving)
    {
        string actionState = (isMoving ? "Walk" : "Idle");

        switch (dir)
        {
            case AnimationDir.Up:
                animator.Play("Up" + actionState);
                break;
            case AnimationDir.Left:
                animator.Play("Left" + actionState);
                break;
            case AnimationDir.Down:
                animator.Play("Down" + actionState);
                break;
            case AnimationDir.Right:
                animator.Play("Right" + actionState);
                break;
        }
    }

    private AnimationDir MoveDir2AnimDir(Move.Direction dir)
    {
        switch (dir)
        {
            case Move.Direction.Left:
                return AnimationDir.Left;
            case Move.Direction.Right:
                return AnimationDir.Right;
            case Move.Direction.Up:
                return AnimationDir.Up;
            case Move.Direction.Down:
                return AnimationDir.Down;
            default:
                return AnimationDir.Down;
        }
    }
}

public enum AnimationDir
{
    Up,
    Left,
    Down,
    Right
}
