using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public SpriteRenderer selector;

    public void Select()
    {
        selector.enabled = true;
    }

    public void Deselect()
    {
        selector.enabled = false;
    }
}
