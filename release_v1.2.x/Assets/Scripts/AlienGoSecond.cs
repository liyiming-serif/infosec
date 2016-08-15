using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AlienGoSecond : AIGoScript {

    public override void Run(CivilianC civilian, ServersGraphC serversC, List<Slot> slots, bool isForward)
    {
        Domain d;
        if (step == -1)
        {
            slots.Reverse();
            d = slots[step + 1].holding;
            if (d.dName == "CITI")
            {
                civilian.GetConfused();
            }
            else // "COM"
            {
                d.GetComponent<Image>().color = Color.green;
                serversC.LightupDomainName("COM", Color.green);
                serversC.ActivatePath("COM", true);
                civilian.SetEndPosition(serversC.GetLandingPos("COM"));
                step += 1;
            }
        }
        else if (step == 0)
        {
            d = slots[step + 1].holding;
            if (d.dName == "COM")
            {
                civilian.GetConfused();
            }
            else // "CITI"
            {
                d.GetComponent<Image>().color = Color.green;
                serversC.LightupDomainName("CITI", Color.green);
                serversC.ActivatePath("CITI", true);
                civilian.SetEndPosition(serversC.GetLandingPos("CITI"));
                step += 1;
            }
        }
        else if (step == 1)
        {
            serversC.ActivatePath("CITI", false);
            civilian.GetExploded(delegate { });
        }
    }
}