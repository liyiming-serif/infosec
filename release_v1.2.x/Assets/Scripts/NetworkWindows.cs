using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class NetworkWindows : GUI, IEventSystemHandler
{
    public static NetworkWindows instance;
    
    [SerializeField]
    GameObject urlString;

    AlienGoScript alienGo; 
    AlienC alienC;
    ServersGraphC serversC;
    List<Slot> slots;
    
    new void Awake()
    {
        base.Awake();
        Register(GetHashCode());
        slots = new List<Slot>();
        slots.AddRange(urlString.GetComponentsInChildren<Slot>());
        instance = this;
    }

    void Start()
    {
        serversC = GetComponentInChildren<ServersGraphC>();
        alienC = GetComponentInChildren<AlienC>();
        alienGo = GetComponent<AlienGoScript>();
    }

    public void ResetAlienGo()
    {
        alienGo.Reset(slots);
    }

    public void AlienGo(bool isForward)
    {
        alienGo.Run(alienC, serversC, slots, isForward);
    }

    public void updateNetworkURL(Domain d, int id)
    {
        Domain tobeDestroyed = slots[id].holding;
        if(tobeDestroyed)
        {
            Destroy(tobeDestroyed.gameObject);
        }
        Instantiate(d, slots[id].transform);
    }

}
