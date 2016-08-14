using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AlienGoScript : MonoBehaviour {

    protected int step;
    protected Domain d;

    public virtual void Run(AlienC alienC, ServersGraphC serversC, List<Slot> slots)
    {
        Debug.Log("Control the Alien phase.");
    }

    public void Reset(List<Slot> slots)
    {
        if(step >= 0)
        {
            step = -1;
            slots.Reverse(); //Enable sync.
        }
    }

    public void Animate(AlienC alienC, ServersGraphC serversC)
    {
        d.GetComponent<Image>().color = Color.green;
        serversC.LightupDomainName(d.dName);
        serversC.ActivatePath(d.dName, true);
        alienC.SetEndPosition(serversC.GetLandingPos(d.dName));
        step += 1;
    }

    void Awake()
    {
        step = -1;
        d = null;
    }
}
