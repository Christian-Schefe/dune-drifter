using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    public GameObject tempPointer;

    private Match match;
    private Coroutine matchCoroutine;

    public void OnMatchBegin(Match match)
    {
        this.match = match;
        matchCoroutine = StartCoroutine(MatchCoroutine());
    }

    public void OnMatchEnd(Match match)
    {
        StopCoroutine(matchCoroutine);
        this.match = null;
    }

    private void OnEnable()
    {
        Signals.Get<MatchSignal.Start>().AddListener(OnMatchBegin);
    }

    private void OnDisable()
    {
        Signals.Get<MatchSignal.End>().RemoveListener(OnMatchEnd);
    }

    private IEnumerator MatchCoroutine()
    {
        while (true)
        {
            var mouse = Input.mousePosition;
            var mouseRay = Globals.Get<Camera>().ScreenPointToRay(mouse);

            var grid = Globals.Get<Main>().grid;

            var roundedHex = grid.WorldRayToHexRound(mouseRay);
            var roundedWorld = grid.HexToWorld(roundedHex);

            tempPointer.transform.position = roundedWorld;

            if (Input.GetMouseButtonDown(0))
            {
                Events.Get<PlayCardCommand>().Dispatch((0, new PlayCardTarget.SingleFreeField(roundedHex)));
            }

            yield return null;
        }
    }
}
