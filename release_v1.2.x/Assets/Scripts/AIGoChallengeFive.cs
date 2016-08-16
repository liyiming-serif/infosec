using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class AIGoChallengeFive : AIGoScript
{

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
                    if (d.myName == "CITI" || d.myName == "ONLINE")
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
                    d = slots[step + 1].holding;
                    if (d.myName == "CITI" || d.myName == "ONLINE")
                    {
                        serversC.goingTo = serversC.nowAt.ReturnChild(d.myName);
                        Animate(alien, serversC, true);
                    }
                    else
                    {
                        alien.GetConfused();
                    }
                    break;
                case 2:
                    serversC.nowAt = serversC.goingTo;
                    serversC.nowAt.BucklePath(false);
                    if (d.myName == "CITI")
                    {
                        alien.GetExploded(delegate { GetComponent<NetworkWindows>().NextAI(delegate { Feedback.instance.popUp(true, "Challenge5"); }); });
                    }
                    else if (d.myName == "ONLINE")
                    {
                        alien.GetRevenge(delegate { Feedback.instance.popUp(false, "Challenge5"); });
                    }
                    else
                    {
                        alien.GetConfused();
                    }
                    break;
                default:
                    Debug.Log("Shouldn't reach here.@AIGoChallengeFour");
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