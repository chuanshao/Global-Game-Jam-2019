using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmileGame;

/// <summary>
/// Game input
/// </summary>
public class GameInput : MonoSingleton<GameInput>
{
    public static float Horizontal
    {
        get { return Input.GetAxis("Horizontal"); }
    }

    public static float Vertical
    {
        get { return Input.GetAxis("Vertical"); }
    }

}
