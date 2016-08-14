using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AlienGoThird : AlienGoScript
{
    // Three choices: COM, CITI and BANK.
    public override void Run(AlienC alienC, ServersGraphC serversC, List<Slot> slots)
    {

        switch (step)
        {
            case -1:
                slots.Reverse();//Read from the end.
                d = slots[step + 1].holding;
                if (d.dName == "COM")
                {
                    d.GetComponent<Image>().color = Color.green;
                    serversC.LightupDomainName(0);
                    serversC.ActivatePath(0, true);
                    alienC.SetEndPosition(serversC.GetLandingPos(0));
                    step += 1;
                }
                else
                {
                    // Invalid 
                    alienC.GetConfused();
                    slots.Reverse(); //Enable sync.
                }
                break;
            case 0:
                serversC.ActivatePath(0, false);
                d = slots[step + 1].holding;
                if (d.dName == "CITI")
                {
                    d.GetComponent<Image>().color = Color.green;
                    serversC.LightupDomainName(1);
                    serversC.ActivatePath(1, true);
                    alienC.SetEndPosition(serversC.GetLandingPos(1));
                    step += 1;
                }
                else if (d.dName == "BANK")
                {
                    d.GetComponent<Image>().color = Color.green;
                    serversC.LightupDomainName(2);
                    serversC.ActivatePath(2, true);
                    alienC.SetEndPosition(serversC.GetLandingPos(2));
                    step += 1;
                }
                else
                {
                    alienC.GetConfused();
                    slots.Reverse(); //Enable sync.
                }
                break;
            case 1:
                if (d.dName == "CITI")
                {
                    serversC.ActivatePath(1, false);
                    alienC.GetExploded();

                }
                else if (d.dName == "BANK")
                {

                    serversC.ActivatePath(2, false);
                    alienC.GetConfused();
                }
                else
                {
                    alienC.GetConfused();
                }
                slots.Reverse(); //Enable sync.
                break;
        }
    }
}