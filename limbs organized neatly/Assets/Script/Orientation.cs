using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum orientation
{
    East = 0,
    South = 1,
    West = 2,
    North = 3,
}

public static class Orientation
{
    public static orientation GetNextOrientation(orientation orientation)
    {
        switch (orientation)
        {
            default:

            case orientation.East:
                return orientation.South;

            case orientation.South:
                return orientation.West;

            case orientation.West:
                return orientation.North;

            case orientation.North:
                return orientation.East;
        }
    }

    public static int GetOrientationAngle(orientation orientation)
    {
        switch (orientation)
        {
            default:

            case orientation.East:
                return 0;

            case orientation.North:
                return 90;

            case orientation.West:
                return 180;

            case orientation.South:
                return 270;
        }
    }
    public static orientation GetOrientationWithinOrientation(orientation innerOrientation, orientation outerOrientation)
    {
        int additiveValueIndex = (int)innerOrientation + (int)outerOrientation;

        Type type = typeof(orientation);
        Array values = type.GetEnumValues();
        if (additiveValueIndex < values.Length)
        {
            return (orientation)values.GetValue(additiveValueIndex);
        }
        else
        {
            return (orientation)values.GetValue(additiveValueIndex - values.Length);
        }
    }

    public static orientation GetRandomOrientation()
    {
        Type type = typeof(orientation);
        Array values = type.GetEnumValues();
        int index = UnityEngine.Random.Range(0, values.Length);
        orientation value = (orientation)values.GetValue(index);
        return value;
    }

    public static Vector2 GetRotatedPoint(Vector2 orientationEastPoint, orientation orientation)
    {
        Vector2 rotatedGridPoint = new Vector2();

        switch (orientation)
        {
            case orientation.East:
                rotatedGridPoint = orientationEastPoint;
                break;

            case orientation.South:
                rotatedGridPoint = new Vector2(orientationEastPoint.y, -orientationEastPoint.x);
                break;

            case orientation.West:
                rotatedGridPoint = new Vector2(-orientationEastPoint.x, -orientationEastPoint.y);
                break;

            case orientation.North:
                rotatedGridPoint = new Vector2(-orientationEastPoint.y, orientationEastPoint.x);
                break;
        }

        return rotatedGridPoint;
    }

    public static Vector2 GetRotatedPointInSquareShape(Vector2 orientationEastPoint, float squareWidth, orientation orientation)
    {
        Vector2 rotatedGridPoint = new Vector2();

        switch (orientation)
        {
            case orientation.East:
                rotatedGridPoint = orientationEastPoint;
                break;

            case orientation.South:
                rotatedGridPoint = new Vector2(orientationEastPoint.y, squareWidth - 1 - orientationEastPoint.x);
                break;

            case orientation.West:
                rotatedGridPoint = new Vector2(squareWidth - 1 - orientationEastPoint.x, squareWidth - 1 - orientationEastPoint.y);
                break;

            case orientation.North:
                rotatedGridPoint = new Vector2(squareWidth - 1 - orientationEastPoint.y, orientationEastPoint.x);
                break;
        }

        return rotatedGridPoint;
    }

    #region EDITOR SCRIPT
#if UNITY_EDITOR
    public static Vector2 GetRotatedPointGUI(Vector2 orientationEastPoint, orientation orientation)
    {
        Vector2 rotatedGridPoint = new Vector2();

        switch (orientation)
        {
            case orientation.East:
                rotatedGridPoint = orientationEastPoint;
                break;

            case orientation.South:
                rotatedGridPoint = new Vector2(-orientationEastPoint.y, orientationEastPoint.x);
                break;

            case orientation.West:
                rotatedGridPoint = new Vector2(-orientationEastPoint.x, -orientationEastPoint.y);
                break;

            case orientation.North:
                rotatedGridPoint = new Vector2(orientationEastPoint.y, -orientationEastPoint.x);
                break;
        }

        return rotatedGridPoint;
    }
#endif
    #endregion EDITOR SCRIPT
}
