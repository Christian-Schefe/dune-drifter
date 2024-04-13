using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCard
{
    public virtual void Execute(Match match)
    {
        Log.Debug("Execute");
    }
}
