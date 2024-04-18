using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : GameObserver
{
    public Tile tilePrefab;

    private readonly Dictionary<Vector2Int, Tile> tiles = new();

    private readonly HashSet<Vector2Int> selectedTiles = new();

    protected override void OnMatchStart()
    {
        foreach (var tile in Arena.grid.data.Keys)
        {
            var instance = Instantiate(tilePrefab, transform);
            instance.Deselect();
            var worldPos = Grid.HexToWorld(tile);
            instance.transform.position = worldPos;
            tiles.Add(tile, instance);
        }
    }

    protected override void OnMatchEnd()
    {
        foreach (var tile in tiles)
        {
            Destroy(tile.Value.gameObject);
        }
        tiles.Clear();
    }

    public void Select(Vector2Int pos)
    {
        var tile = tiles[pos];
        tile.Select();
        selectedTiles.Add(pos);
    }

    public void Deselect(Vector2Int? pos = null)
    {
        if (pos is Vector2Int sp)
        {
            tiles[sp].Deselect();
            selectedTiles.Remove(sp);
        }
        else
        {
            foreach (var p in selectedTiles)
            {
                tiles[p].Deselect();
            }
            selectedTiles.Clear();
        }
    }
}
