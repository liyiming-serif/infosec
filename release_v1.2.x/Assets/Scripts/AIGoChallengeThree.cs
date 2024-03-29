﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AIGoChallengeThree : AIGoScript
{
    // Three choices: COM, CITI and BANK.
    public override void Run(AlienC alien, ServersGraphC serversC, List<Slot> slots, bool isForward)
    {
        if (isForward)
        {
            switch (step)
            {
                case -1:
                    slots.Reverse();//Read from the end.
                    d = slots[step + 1].holding;
                    if (d.myName == "COM")
                    {
                        Animate(alien, serversC, true);
                    }
                    else
                    {
                        alien.GetConfused();
                    }
                    break;
                case 0:
                    serversC.nowAt = serversC.goingTo;
                    serversC.nowAt.BucklePath(false);
                    d = slots[step + 1].holding;
                    if (d.myName == "CITI" || d.myName == "C1TI")
                    {
                        serversC.goingTo = serversC.nowAt.ReturnChild(d.myName);
                        Animate(alien, serversC, true);
                    }
                    else
                    {
                        alien.GetConfused();
                    }
                    break;
                case 1:
                    serversC.nowAt = serversC.goingTo;
                    serversC.nowAt.BucklePath(false);
                    if (d.myName == "CITI")
                    {
                        alien.GetRevenge(delegate { Feedback.instance.popUp(false, "Challenge3"); });
                    }
                    else if (d.myName == "C1TI")
                    {
                        alien.GetExploded(delegate { Feedback.instance.popUp(true, "Challenge5"); });
                    }
                    else
                    {
                        Debug.Log("Shouldn't reach here.@AIGoChallengeThree");
                    }
                    break;
                default:
                    Debug.Log("Shouldn't reach here.@AIGoChallengeThree");
                    break;
            }
        }
        else
        {
            if (step > 0)
            {
                d = slots[step - 1].holding;
                serversC.goingTo = serversC.nowAt.ReturnParent();
                Animate(alien, serversC, false);
            }
            else if (step == 0)
            {
                serversC.nowAt = serversC.goingTo;
                serversC.goingTo = serversC.nowAt.ReturnParent();
                Animate(alien, serversC, false);
            }
            else
            {
                //Reached the launchpad.
                serversC.nowAt = serversC.goingTo;
                serversC.goingTo = serversC.root;
                alien.isForward = true;
                slots.Reverse(); //Enable sync.
                Hint();
            }
        }

    }
}