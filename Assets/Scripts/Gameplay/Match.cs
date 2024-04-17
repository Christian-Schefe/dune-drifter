using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchParams
{
    public int arenaSize;
    public int handSize;
}

public class Match
{
    public Arena arena;
    public Hand hand;
    public Run run;

    public Match(Run run, Deck deck, MatchParams matchParams)
    {
        this.run = run;
        arena = new(matchParams.arenaSize);
        hand = new(deck, matchParams.handSize);

        Queries.Get<PlayCardCommand>().SetResponder(OnPlayCard);
    }

    public CommandResponse OnPlayCard((int index, PlayCardTarget target) args)
    {
        var card = hand.GetCard(args.index);
        if (card.Execute(this, args.target))
        {
            hand.PlayCard(args.index);
            return CommandResponse.Ok;
        }
        else
        {
            return CommandResponse.BadRequest;
        }
    }
}
