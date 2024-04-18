using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameObserver : MonoBehaviour
{
    public Main Main => Globals.Get<Main>();
    public Run Run => Main.run;
    public Match Match => Run.currentMatch;
    public Arena Arena => Match.arena;
    public HexGrid Grid => Globals.Get<HexGrid>();
    public bool CurrentPlayer => Match.currentPlayer;

    private GameEvents events;

    private void OnEnable()
    {
        events = Main.events;
        events.runStart += OnRunStart;
        events.runEnd += OnRunEnd;
        events.matchStart += OnMatchStart;
        events.matchEnd += OnMatchEnd;
        events.turnStart += OnTurnStart;
        events.turnEnd += OnTurnEnd;
        events.pieceSpawned += OnPieceSpawned;
        events.pieceMoved += OnPieceMoved;
        events.pieceAttacked += OnPieceAttacked;
        events.pieceDied += OnPieceDied;
        events.pieceLostShield += OnPieceLostShield;
        events.playCardDrawn += OnPlayCardDrawn;
        events.playCardPlayed += OnPlayCardPlayed;
    }

    private void OnDisable()
    {
        events.runStart -= OnRunStart;
        events.runEnd -= OnRunEnd;
        events.matchStart -= OnMatchStart;
        events.matchEnd -= OnMatchEnd;
        events.turnStart -= OnTurnStart;
        events.turnEnd -= OnTurnEnd;
        events.pieceSpawned -= OnPieceSpawned;
        events.pieceMoved -= OnPieceMoved;
        events.pieceAttacked -= OnPieceAttacked;
        events.pieceDied -= OnPieceDied;
        events.pieceLostShield -= OnPieceLostShield;
        events.playCardDrawn -= OnPlayCardDrawn;
        events.playCardPlayed -= OnPlayCardPlayed;
    }

    protected virtual void OnRunStart() { }

    protected virtual void OnRunEnd() { }

    protected virtual void OnMatchStart() { }

    protected virtual void OnMatchEnd() { }

    protected virtual void OnTurnStart() { }

    protected virtual void OnTurnEnd() { }

    protected virtual void OnPieceSpawned(Piece piece, Vector2Int at) { }

    protected virtual void OnPieceMoved(Piece piece, Vector2Int from, Vector2Int to) { }

    protected virtual void OnPieceAttacked(Piece piece, Vector2Int from, Vector2Int to) { }

    protected virtual void OnPieceDied(Piece piece, Vector2Int at) { }

    protected virtual void OnPieceLostShield(Piece piece, Vector2Int at) { }

    protected virtual void OnPlayCardDrawn(PlayCard card) { }

    protected virtual void OnPlayCardPlayed(PlayCard card) { }

    protected Vector2Int MouseHex
    {
        get
        {
            var camera = Globals.Get<Camera>();
            var mouseScreen = Input.mousePosition;
            var mouseWorldRay = camera.ScreenPointToRay(mouseScreen);
            var gridIntersect = Grid.RayIntersectRound(mouseWorldRay);
            return gridIntersect;
        }
    }
}
