using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AIGoScript : MonoBehaviour {

    protected int step;
    protected Domain d;

    public virtual void Hint()
    {
        TaskManager.instance.LookUpAppSpawn("Ping!").Dance();
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
            Run((aiC as AlienC), serversC, slots, isForward);
        }
        else
        {
            Run((aiC as CivilianC), serversC, slots, isForward);
        }
    }

    public void Animate(AIController ai, ServersGraphC serversC, bool isForward)
    {
        if (isForward)
        {
            serversC.goingTo.BucklePath(true);
            serversC.goingTo.SetDomainColour(serversC.GetLightedColour());
            d.GetComponent<Image>().color = Color.green;
            ai.SetEndPosition(serversC.goingTo.GetLandingPos());
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
                serversC.goingTo.BucklePath(true);
                ai.SetEndPosition(serversC.goingTo.GetLandingPos());
            }
            step -= 1;
        }
    }

    public void Reset()
    {
        step = -1;
        d = null;
    }

    protected void Awake()
    {
        Reset();
    }

    protected void Start()
    {
        TaskManager.instance.LookUpAppSpawn("Ping!").Launch();
    }
}
