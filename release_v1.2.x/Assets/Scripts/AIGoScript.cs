using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AIGoScript : MonoBehaviour {

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

    public virtual void Run(CivilianC alienC, ServersGraphC serversC, List<Slot> slots, bool isForward)
    {
        Debug.Log("Control the civilian phase.");
    }

    public void Run(AIController aiC, ServersGraphC serversC, List<Slot> slots, bool isForward)
    {
        if (aiC is AlienC)
        {
            Run((aiC as AlienC), serversC, slots, true);
        }
        else
        {
            Run((aiC as CivilianC), serversC, slots, true);
        }
    }

    public void Animate(AIController ai, ServersGraphC serversC, bool isForward)
    {
        serversC.ActivatePath(d.dName, true);
        if (isForward)
        {
            serversC.LightupDomainName(d.dName, Color.green);
            d.GetComponent<Image>().color = Color.green;
            ai.SetEndPosition(serversC.GetLandingPos(d.dName));
            step += 1;
        }
        else
        {
            if(step == 0)
            {
                //ToLaunchPad
                ai.ReturnToInitPosition();
            }
            else
            {
                ai.SetEndPosition(serversC.GetLandingPos(d.dName));
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
