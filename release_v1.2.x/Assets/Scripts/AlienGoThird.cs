using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AlienGoThird : AlienGoScript
{
    // Three choices: COM, CITI and BANK.
    public override void Run(AlienC alienC, ServersGraphC serversC, List<Slot> slots, bool isForward)
    {
        if (isForward)
        {
            switch (step)
            {
                case -1:
                    slots.Reverse();//Read from the end.
                    d = slots[step + 1].holding;
                    if (d.dName == "COM")
                    {
                        Animate(alienC, serversC);
                    }
                    else
                    {
                        alienC.GetConfused();
                    }
                    break;
                case 0:
                    serversC.ActivatePath(d.dName, false);
                    d = slots[step + 1].holding;
                    if (d.dName == "CITI" || d.dName == "BANK")
                    {
                        Animate(alienC, serversC);
                    }
                    else
                    {
                        alienC.GetConfused();
                    }
                    break;
                case 1:
                    serversC.ActivatePath(d.dName, false);
                    if (d.dName == "CITI")
                    {
                        alienC.GetExploded(delegate { Feedback.instance.popUp(true, "Challenge4"); });
                    }
                    else if (d.dName == "BANK")
                    {
                        alienC.GetConfused();
                    }
                    else
                    {
                        alienC.GetConfused();
                    }
                    break;
                default:
                    Debug.Log("Shouldn't reach here.@AlienGoThird");
                    break;
            }
        }
        else
        {
            switch (step)
            {
                case 1:
                    Debug.Log("hello");
                    break;
                case 0:
                    break;
                case -1:
                    break;
                default:
                    Debug.Log("Shouldn't reach here.@AlienGoThird");
                    break;
            }
        }

    }
}