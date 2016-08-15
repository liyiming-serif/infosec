using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AlienGoScript : MonoBehaviour {

    protected int step;
    protected Domain d;

    public virtual void Hint()
    {
        Debug.Log("Shake the app spawn icon.");
    }

    public virtual void Run(AlienC alienC, ServersGraphC serversC, List<Slot> slots, bool isForward)
    {
        Debug.Log("Control the Alien phase.");
    }

    public void Animate(AlienC alienC, ServersGraphC serversC, bool isForward)
    {
        serversC.ActivatePath(d.dName, true);
        if (isForward)
        {
            serversC.LightupDomainName(d.dName, Color.green);
            d.GetComponent<Image>().color = Color.green;
            alienC.SetEndPosition(serversC.GetLandingPos(d.dName));
            step += 1;
        }
        else
        {
            if(step == 0)
            {
                //ToLaunchPad
                alienC.ReturnToInitPosition();
            }
            else
            {
                alienC.SetEndPosition(serversC.GetLandingPos(d.dName));
            }
            step -= 1;
        }
        
    }

    public void Reset()
    {
        step = -1;
        d = null;
    }

    void Awake()
    {
        Reset();
    }
}
