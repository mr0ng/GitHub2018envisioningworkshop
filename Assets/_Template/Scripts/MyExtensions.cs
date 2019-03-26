using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerColorEnum
{
    Red,
    Blue,
    Green,
    Cyan,
    Yellow,
    White
}


public enum MySpatialButtonState
{
    Closed,
    RecordFront,
    StopRecordFront,
    CloseIconFront
}

public static class MyExtensions{

    public static Color ToColor(this PlayerColorEnum colorEnum)
    {
        Color color = Color.black;
        switch (colorEnum)
        {
            case PlayerColorEnum.Red:
                color = Color.red;
                break;
            case PlayerColorEnum.Blue:
                color = Color.blue;
                break;
            case PlayerColorEnum.Green:
                color = Color.green;
                break;
            case PlayerColorEnum.Cyan:
                color = Color.cyan;
                break;
            case PlayerColorEnum.Yellow:
                color = Color.yellow;
                break;
            case PlayerColorEnum.White:
                color = Color.white;
                break;
            default:
                break;
        }
        return color;
    }
}
