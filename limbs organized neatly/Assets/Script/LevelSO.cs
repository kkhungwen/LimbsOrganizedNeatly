using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO_", menuName = "Scriptable Objects/Level")]
public class LevelSO : ScriptableObject
{
    public string[] textArray;

    public Vector2Int[] availableSlotArray;
    public LimbOrginPosition[] limbOrginPositionArray;

    public void GetWidthHeight(out int width, out int height)
    {
        width = 0;
        foreach (Vector2Int position in availableSlotArray)
        {
            if (position.x + 1 > width)
                width = position.x + 1;
        }

        height = 0;
        foreach (Vector2Int position in availableSlotArray)
        {
            if (position.y + 1 > height)
                height = position.y + 1;
        }
    }
}

[System.Serializable]
public class LimbOrginPosition
{
    public LimbSO limbSO;
    public Vector2 orginPosition;
}
