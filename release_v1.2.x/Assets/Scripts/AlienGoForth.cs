using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class AlienGoForth : AlienGoScript {

    public override void Hint()
    {
        TaskManager.instance.LookUpAppSpawn("Ping!").Dance();
    }

    //TODO Change the Run method
    public override void Run(AlienC alienC, ServersGraphC serversC, List<Slot> slots, bool isForward)
    {
        Domain d;
        if (step == -1)
        {
            slots.Reverse();
            d = slots[step + 1].holding;
            if (d.dName == "C1TI")
            {
                //alienC.GetConfused();
            }
            else // "COM"
            {
                d.GetComponent<Image>().color = Color.green;
                serversC.LightupDomainName(d.dName, Color.green);
                serversC.ActivatePath(d.dName, true);
                alienC.SetEndPosition(serversC.GetLandingPos(d.dName));
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
            else if(d.dName == "CITI") // "CITI"
            {
                d.GetComponent<Image>().color = Color.green;
                serversC.LightupDomainName(d.dName, Color.green);
                serversC.ActivatePath(d.dName, true);
                alienC.SetEndPosition(serversC.GetLandingPos(d.dName));
                step += 1;
            }
        }
        else if (step == 1)
        {
            d = slots[step].holding;
            serversC.ActivatePath(d.dName, false);
            alienC.GetExploded(delegate { GetComponent<NetworkWindows>().NextAI(delegate { Feedback.instance.popUp(true, "Challenge4"); }); });
        }
    }
}