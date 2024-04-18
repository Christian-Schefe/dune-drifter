using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputU
{
    public static Vector3 MouseWorld
    {
        get
        {
            return Globals.Get<Camera>().ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public static Ray MouseRay
    {
        get
        {
            return Globals.Get<Camera>().ScreenPointToRay(Input.mousePosition);
        }
    }
}
