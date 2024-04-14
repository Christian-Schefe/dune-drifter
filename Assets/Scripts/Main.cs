using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Run run;
    public HexGrid grid;

    private void Start()
    {
        grid = new(transform);

        run = new();
        run.BeginMatch();
    }

    private void OnDrawGizmos()
    {
        grid = new(transform);

        Gizmos.color = Color.green;
        for (int x = -10; x < 10; x++)
        {
            for (int y = -10; y < 10; y++)
            {
                var vec = new Vector2Int(x, y);
                var pos = grid.HexToWorld(vec);
                var pos2 = grid.HexToWorld(grid.WorldToHex(pos));
                var dist = HexGrid.ManhattenDistance(Vector2Int.zero, vec);

                if (dist <= 4)
                {
                    Gizmos.DrawWireSphere(pos2, 0.3f);
                }
            }
        }
        var origin = grid.transform.Position;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + grid.transform.Up);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin, origin + grid.transform.Forward);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + grid.transform.Right);
    }
}