using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtils
{
    public static Vector2 GetMouseWorldPosition()
    {
        return GetWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static Vector3 GetScreenLowerBoundWorldPosition()
    {
        Vector3 vec = GetWorldPositionWithZ(Vector3.zero, Camera.main);
        vec.z = 0;
        return vec;
    }

    public static Vector3 GetScreenUpperBoundWorldPosition()
    {
        Vector3 vec = GetWorldPositionWithZ(new Vector3(Screen.width, Screen.height, 0), Camera.main);
        vec.z = 0;
        return vec;
    }

    public static bool TryGetComponentInChildrenDepth1<T>(Transform transform, out T value) where T : Component
    {
        T[] componentArray = transform.GetComponentsInChildren<T>();

        foreach (T component in componentArray)
        {
            if (component.transform.parent == transform)
            {
                value = component;
                return true;
            }
        }

        value = null;
        return false;
    }

    public static bool TrayGetComponentInSameDepth<T>(Transform transform, out T value) where T : Component
    {
        return (TryGetComponentInChildrenDepth1<T>(transform.parent, out value));
    }

    public static float LinearToDecibels(int linear)
    {
        float linearScaleRange = 20f;
        return Mathf.Log10((float)linear / linearScaleRange) * 20f;
    }

    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);

        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }

    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        return directionVector;
    }

    public static Vector2 GetTrajectoryVelocity(Vector2 startPosition, Vector2 targetPosition, float time)
    {
        Vector2 velocity = targetPosition / time - startPosition / time - 0.5f * Physics2D.gravity * time;
        return velocity;
    }


    #region Validation
    public static bool ValidateCheckEmptyString(Object thisObject, string fileName, string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log(fileName + " is empty and must contain a value in object " + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    public static bool ValidateCheckNullValue(Object thisObject, string fileName, UnityEngine.Object objectToCheck)
    {
        if (objectToCheck == null)
        {
            Debug.Log(fileName + " is null " + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    public static bool ValidateCheckEnumerableValues(Object thisObject, string fileName, IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjectToCheck == null)
        {
            Debug.Log(fileName + " is null " + thisObject.name.ToString());
            return true;
        }

        foreach (var item in enumerableObjectToCheck)
        {
            if (item == null)
            {
                Debug.Log(fileName + " has null value in object " + thisObject.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }
        }

        if (count == 0)
        {
            Debug.Log(fileName + " has no values in object " + thisObject.name.ToString());
            error = true;
        }

        return error;
    }

    public static bool ValidateCheckPositiveValue(Object thisObject, string fileName, int valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                error = true;
            }
        }

        return error;
    }

    public static bool ValidateCheckPositiveValue(Object thisObject, string fileName, float valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                error = true;
            }
        }

        return error;
    }

    public static bool ValidateCheckPositiveRange(Object thisObject, string fieldNameMinimum, float valueToCheckMinimum, string fieldNameMaximum, float valueToCheckMaximum, bool isZeroAllowed)
    {
        bool error = false;
        if (valueToCheckMinimum > valueToCheckMaximum)
        {
            Debug.Log(fieldNameMinimum + " must be less or equal to " + fieldNameMaximum + " in object " + thisObject.name.ToString());
            error = true;
        }

        if (ValidateCheckPositiveValue(thisObject, fieldNameMinimum, valueToCheckMinimum, isZeroAllowed)) error = true;
        if (ValidateCheckPositiveValue(thisObject, fieldNameMaximum, valueToCheckMaximum, isZeroAllowed)) error = true;

        return error;
    }

    public static void ValidateNormalizedCurve(AnimationCurve animationCurve)
    {
        if (animationCurve.length < 2)
        {
            animationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });
        }

        Keyframe startKey = animationCurve.keys[0];
        startKey.time = 0;
        startKey.value = 0;
        animationCurve.MoveKey(0, startKey);

        Keyframe endKey = animationCurve.keys[animationCurve.keys.Length - 1];
        endKey.time = 1;
        endKey.value = 1;
        animationCurve.MoveKey(animationCurve.keys.Length - 1, endKey);
    }
    #endregion Validation
}
