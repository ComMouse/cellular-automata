using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public InputType type = InputType.None;

    public Vector2 GetAxis()
    {
        return new Vector2(
            Input.GetAxis(GetInputName("Horizontal")),
            Input.GetAxis(GetInputName("Vertical")));
    }

    public bool GetButton1Down()
    {
        return Input.GetButton(GetInputName("Fire1"));
    }

    public bool GetButton2Down()
    {
        return Input.GetButton(GetInputName("Fire2"));
    }

    public bool GetRestartDown()
    {
        return Input.GetButton("Restart");
    }

    public string GetInputName(string key)
    {
        return $"{key} {(int)type}";
    }
}

public enum InputType
{
    None,
    Keyboard1 = 1,
    Keyboard2 = 2,
    Console1 = 3,
    Console2 = 4,
    Console3 = 5,
    Console4 = 6
}