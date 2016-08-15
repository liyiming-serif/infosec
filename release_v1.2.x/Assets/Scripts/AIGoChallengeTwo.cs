using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AIGoChallengeTwo : AIGoScript {

    public override void Run(CivilianC civilian, ServersGraphC serversC, List<Slot> slots, bool isForward)
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
                        Animate(civilian, serversC, true);
                    }
                    else
                    {
                        civilian.GetConfused();
                    }
                    break;
                case 0:
                    serversC.ActivatePath(d.dName, false);
                    d = slots[step + 1].holding;
                    if (d.dName == "CITI")
                    {
                        Animate(civilian, serversC, true);
                    }
                    else
                    {
                        civilian.GetConfused();
                    }
                    break;
                case 1:
                    civilian.BecomeSafe(delegate { Feedback.instance.popUp(true, "Challenge3"); });
                    break;
                default:
                    Debug.Log("Shouldn't reach here.@AIGoChallengeTwo");
                    break;
            }
        }
        else
        {
            if (step > 0)
            {
                d = slots[step - 1].holding;
                Animate(civilian, serversC, false);
            }
            else if (step == 0)
            {
                Animate(civilian, serversC, false);
            }
            else
            {
                //Reached the launchpad.
                civilian.isForward = true;
                slots.Reverse(); //Enable sync.
            }
        }
    }
}