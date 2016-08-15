using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AlienGoFirst : AlienGoScript {

    public override void Run(AlienC alienC, ServersGraphC serversC, List<Slot> slots, bool isForward)
    {
        if (step == -1)
        {
            slots[step + 1].holding.GetComponent<Image>().color = Color.green;
            serversC.LightupDomainName("COM", Color.green);
            serversC.ActivatePath("COM", true);
            alienC.SetEndPosition(serversC.GetLandingPos("COM"));
            step += 1;
        }
        else if (step == 0)
        {
            serversC.ActivatePath("COM", false);
            alienC.GetExploded(delegate { Feedback.instance.popUp(true, "Challenge2"); });
        }
    }

}