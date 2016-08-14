using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AlienGoForth : AlienGoScript {
    //TODO Change the Run method
    public override void Run(AlienC alienC, ServersGraphC serversC, List<Slot> slots)
    {
        Domain d;
        if (nowAt == -1)
        {
            slots.Reverse();
            d = slots[nowAt + 1].holding;
            if (d.dName == "CITI")
            {
                //alienC.GetConfused();
            }
            else // "COM"
            {
                d.GetComponent<Image>().color = Color.green;
                serversC.LightupDomainName(nowAt + 1);
                serversC.ActivatePath(nowAt + 1, true);
                alienC.SetEndPosition(serversC.GetLandingPos(nowAt + 1));
                nowAt += 1;
            }
        }
        else if (nowAt == 0)
        {
            d = slots[nowAt + 1].holding;
            if (d.dName == "COM")
            {
                //alienC.GetConfused();
            }
            else // "CITI"
            {
                d.GetComponent<Image>().color = Color.green;
                serversC.LightupDomainName(nowAt + 1);
                serversC.ActivatePath(nowAt + 1, true);
                alienC.SetEndPosition(serversC.GetLandingPos(nowAt + 1));
                nowAt += 1;
            }
        }
        else if (nowAt == 1)
        {
            serversC.ActivatePath(nowAt, false);
            alienC.GetExploded();
        }
    }
}