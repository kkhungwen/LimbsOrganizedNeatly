using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="LimbSO_",menuName ="Scriptable Objects/Limb")]
public class LimbSO : ScriptableObject
{
    [Tooltip("occupied slot when orientaition east, slot(0,0) is the center of the limb")]
    public Vector2Int[] occupiedPositionArray;
    public Sprite sprite;
}
