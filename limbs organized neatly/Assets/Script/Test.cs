using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public LimbSO limbSO;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Limb.CreateLimb(limbSO, HelperUtils.GetMouseWorldPosition());
        }
    }
}
