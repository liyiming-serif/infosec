using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AIGoChallengeThree : AIGoScript
{
    // Three choices: COM, CITI and BANK.
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
                    if (d.dName == "CITI" || d.dName == "BANK")
                    {
                        Animate(civilian, serversC, true);
                    }
                    else
                    {
                        civilian.GetConfused();
                    }
                    break;
                case 1:
                    serversC.ActivatePath(d.dName, false);
                    if (d.dName == "CITI")
                    {
                        civilian.BecomeSafe(delegate { Feedback.instance.popUp(true, "Challenge4"); });
                    }
                    else if (d.dName == "BANK")
                    {
                        civilian.GetConfused();
                    }
                    else
                    {
                        civilian.GetConfused();
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
                Animate(civilian, serversC, false);
            }
            else if(step == 0)
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