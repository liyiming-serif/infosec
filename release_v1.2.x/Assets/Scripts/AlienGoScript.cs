using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class AlienGoScript : MonoBehaviour {

    protected int nowAt;

    public virtual void Run(AlienC alienC, ServersGraphC serversC, List<Slot> slots)
    {
        Debug.Log("Control the Alien phase.");
    }

    void Awake()
    {
        nowAt = -1;
    }
}
