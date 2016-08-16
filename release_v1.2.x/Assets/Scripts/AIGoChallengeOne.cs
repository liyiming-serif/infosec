using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AIGoChallengeOne : AIGoScript {

    public override void Run(AlienC alien, ServersGraphC serversC, List<Slot> slots, bool isForward)
    {
        switch (step)
        {
            case -1:
                slots[step + 1].holding.myImage.color = Color.green;
                d = slots[step + 1].holding;
                Animate(alien, serversC, true);
                break;
            case 0:
                serversC.nowAt = serversC.goingTo;
                serversC.nowAt.BucklePath(false);
                alien.GetExploded(delegate { Feedback.instance.popUp(true, "Challenge2"); });
                break;
        }
    }

    new void Start()
    {

    }

}