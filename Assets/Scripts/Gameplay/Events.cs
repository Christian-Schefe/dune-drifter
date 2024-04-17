using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCardCommand : Command<int, PlayCardTarget> { }

public static class MatchSignal
{
    public class Start : Signal<Match> { }
    public class End : Signal<Match> { }
}

public static class PieceSignal
{
    public class Moved : Signal<Vector2Int, Piece> { }
    public class Attacked : Signal<Vector2Int, Piece> { }
    public class LostShield : Signal<Vector2Int, Piece> { }
    public class Spawned : Signal<Vector2Int, Piece> { }
}

public static class HandSignal
{
    public class Draw: Signal<int, Hand> { }
    public class Play : Signal<int, Hand> { }
}

public class GetMoveOptionsQuery : Query<Vector2Int, (List<Vector2Int>, List<Vector2Int>)> { }

public static class Events
{
    public static T Get<T>()
    {
        var type = typeof(T);
        if (typeof(IQuery).IsAssignableFrom(type))
        {
            return (T)Queries.Get(type);
        }
        else if (typeof(ISignal).IsAssignableFrom(type))
        {
            return (T)Signals.Get(type);
        }
        else
        {
            return default;
        }
    }
}