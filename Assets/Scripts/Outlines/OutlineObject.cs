using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class OutlineObject : MonoBehaviour
{
    public int colorIndex;

    private void Update()
    {
        MaterialPropertyBlock block = new();

        Renderer r = GetComponent<Renderer>();

        r.GetPropertyBlock(block);
        var color = colorIndex switch
        {
            0 => Color.red,
            1 => Color.green,
            2 => Color.blue,
            _ => Color.clear,
        };
        block.SetColor("_OutlineColor", color);
        r.SetPropertyBlock(block);
    }
}
