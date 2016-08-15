using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AIGoChallengeOne : AIGoScript {

    public override void Run(CivilianC civilian, ServersGraphC serversC, List<Slot> slots, bool isForward)
    {
        switch (step)
        {
            case -1:
                slots[step + 1].holding.GetComponent<Image>().color = Color.green;
                serversC.LightupDomainName("COM", Color.green);
                serversC.ActivatePath("COM", true);
                civilian.SetEndPosition(serversC.GetLandingPos("COM"));
                step += 1;
                break;
            case 0:
                serversC.ActivatePath("COM", false);
                civilian.BecomeSafe(delegate { Feedback.instance.popUp(true, "Challenge2"); });
                break;
        }
    }

}