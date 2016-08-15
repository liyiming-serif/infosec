using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class AIGoChallengeFour : AIGoScript {

    public override void Hint()
    {
        TaskManager.instance.LookUpAppSpawn("Ping!").Dance();
    }

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
                    if (d.dName == "CITI" || d.dName == "BANK" || d.dName == "C1TI")
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
                        civilian.BecomeSafe(delegate { GetComponent<NetworkWindows>().NextAI(delegate { }); });
                    }
                    else if (d.dName == "C1TI")
                    {
                        civilian.GetExploded(delegate { Feedback.instance.popUp(false, "Challenge4"); });
                    }
                    else
                    {
                        civilian.GetConfused();
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
    
    public override void Run(AlienC alien, ServersGraphC serversC, List<Slot> slots, bool isForward)
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
                        Animate(alien, serversC, true);
                    }
                    else
                    {
                        alien.GetConfused();
                    }
                    break;
                case 0:
                    serversC.ActivatePath(d.dName, false);
                    d = slots[step + 1].holding;
                    if (d.dName == "CITI" || d.dName == "BANK" || d.dName == "C1TI")
                    {
                        Animate(alien, serversC, true);
                    }
                    else
                    {
                        alien.GetConfused();
                    }
                    break;
                case 1:
                    serversC.ActivatePath(d.dName, false);
                    if (d.dName == "CITI")
                    {
                        alien.GetRevenge(delegate { Feedback.instance.popUp(false, "Challenge4"); });
                    }
                    else if (d.dName == "C1TI")
                    {
                        alien.GetExploded(delegate { Feedback.instance.popUp(true, "Challenge4"); });
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
                Animate(alien, serversC, false);
            }
            else if (step == 0)
            {
                Animate(alien, serversC, false);
            }
            else
            {
                //Reached the launchpad.
                alien.isForward = true;
                slots.Reverse(); //Enable sync.
            }
        }
    }
}