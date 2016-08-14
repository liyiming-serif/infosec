using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AlienGoSecond : AlienGoScript {

    public override void Run(AlienC alienC, ServersGraphC serversC, List<Slot> slots)
    {
        Domain d;
        if (step == -1)
        {
            slots.Reverse();
            d = slots[step + 1].holding;
            if (d.dName == "CITI")
            {
                //alienC.GetConfused();
            }
            else // "COM"
            {
                d.GetComponent<Image>().color = Color.green;
                serversC.LightupDomainName(step + 1);
                serversC.ActivatePath(step + 1, true);
                alienC.SetEndPosition(serversC.GetLandingPos(step + 1));
                step += 1;
            }
        }
        else if (step == 0)
        {
            d = slots[step + 1].holding;
            if (d.dName == "COM")
            {
                //alienC.GetConfused();
            }
            else // "CITI"
            {
                d.GetComponent<Image>().color = Color.green;
                serversC.LightupDomainName(step + 1);
                serversC.ActivatePath(step + 1, true);
                alienC.SetEndPosition(serversC.GetLandingPos(step + 1));
                step += 1;
            }
        }
        else if (step == 1)
        {
            serversC.ActivatePath(step, false);
            alienC.GetExploded();
        }
    }
}