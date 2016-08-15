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

    public void AlienGo(bool isForward)
    {
        if (isForward)
        {
            alienGo.Run(alienC, serversC, slots, true);
        }
        else
        {
            foreach (Slot s in slots)
            {
                Domain d = s.holding;
                serversC.LightupDomainName(d.dName, Color.white);
                d.GetComponent<Image>().color = Color.white;
            }
            alienGo.Run(alienC, serversC, slots, false);
        }

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
