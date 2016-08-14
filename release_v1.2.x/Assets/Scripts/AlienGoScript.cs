using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AlienGoScript : MonoBehaviour {

    protected int step;
    protected Domain d;

    public virtual void Run(AlienC alienC, ServersGraphC serversC, List<Slot> slots)
    {
        Debug.Log("Control the Alien phase.");
    }

    void Awake()
    {
        step = -1;
        d = null;
    }
}
