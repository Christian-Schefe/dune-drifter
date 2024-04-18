using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Run run;
    public GameEvents events = new();

    public static GameEvents Events => Globals.Get<Main>().events;

    private void Start()
    {
        run = new();
        run.BeginMatch();
    }
}

public class GameEvents
{
    public Action runStart;
    public Action runEnd;

    public Action matchStart;
    public Action matchEnd;

    public Action turnStart;
    public Action turnEnd;

    public Action<Piece, Vector2Int> pieceSpawned;
    public Action<Piece, Vector2Int, Vector2Int> pieceMoved;
    public Action<Piece, Vector2Int, Vector2Int> pieceAttacked;
    public Action<Piece, Vector2Int> pieceLostShield;
    public Action<Piece, Vector2Int> pieceDied;

    public Action<PlayCard> playCardDrawn;
    public Action<PlayCard> playCardPlayed;

    public GameEvents()
    {
        runStart += () => Log.Info("[Event] Run Started");
        runEnd += () => Log.Info("[Event] Run Ended");
        matchStart += () => Log.Info("[Event] Match Started");
        matchEnd += () => Log.Info("[Event] Match Ended");
        turnStart += () => Log.Info("[Event] Turn Started");
        turnEnd += () => Log.Info("[Event] Turn Ended");

        pieceSpawned += (piece, at) => Log.Info("[Event] Piece Spawned", piece, at);
        pieceMoved += (piece, from, to) => Log.Info("[Event] Piece Moved", piece, from, to);
        pieceAttacked += (piece, from, to) => Log.Info("[Event] Piece Attacked", piece, from, to);
        pieceLostShield += (piece, at) => Log.Info("[Event] Piece Lost Shield", piece, at);
        pieceDied += (piece, at) => Log.Info("[Event] Piece Died", piece, at);

        playCardDrawn += (card) => Log.Info("[Event] Drew Card", card);
        playCardPlayed += (card) => Log.Info("[Event] Played Card", card);
    }
}