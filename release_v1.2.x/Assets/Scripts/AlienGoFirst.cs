using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AlienGoFirst : AlienGoScript {

    public override void Run(AlienC alienC, ServersGraphC serversC, List<Slot> slots)
    {
        if (nowAt == -1)
        {
            slots[nowAt + 1].holding.GetComponent<Image>().color = Color.green;
            serversC.LightupDomainName(nowAt + 1);
            serversC.ActivatePath(nowAt + 1, true);
            alienC.SetEndPosition(serversC.GetLandingPos(nowAt + 1));
            nowAt += 1;
        }
        else if (nowAt == 0)
        {
            serversC.ActivatePath(nowAt, false);
            alienC.GetExploded();
        }
    }

}