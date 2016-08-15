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
    Queue<AlienC> aliensC;
    ServersGraphC serversC;
    List<Slot> slots;
    AlienC _activeAlienC;
    
    public AlienC activeAlienC
    {
        get
        {
            return _activeAlienC;
        }
    }

    new void Awake()
    {
        base.Awake();
        Register(GetHashCode());
        slots = new List<Slot>();
        aliensC = new Queue<AlienC>();
        slots.AddRange(urlString.GetComponentsInChildren<Slot>());
        instance = this;
    }

    void Start()
    {
        serversC = GetComponentInChildren<ServersGraphC>();
        foreach(AlienC ac in GetComponentsInChildren<AlienC>())
        {
            aliensC.Enqueue(ac);
        }
        alienGo = GetComponent<AlienGoScript>();
        _activeAlienC = aliensC.Dequeue();
    }

    public void Reset()
    {
        slots.Reverse();
        foreach(Slot s in slots)
        {
            Domain d = s.holding;
            serversC.LightupDomainName(d.dName, Color.white);
            Destroy(d.gameObject);
        }
        alienGo.Reset();
    }
    public void NextAI(AlienC.CallbackFunct func)
    {
        Destroy(_activeAlienC.gameObject);
        _activeAlienC = null;
        if(aliensC.Count > 0)
        {
            _activeAlienC = aliensC.Dequeue();
            Reset();
            alienGo.Hint();
        }
        else
        {
            func();
        }
    }

    public void AlienGo(bool isForward)
    {
        if (isForward)
        {
            alienGo.Run(_activeAlienC, serversC, slots, true);
        }
        else
        {
            foreach (Slot s in slots)
            {
                Domain d = s.holding;
                serversC.LightupDomainName(d.dName, Color.white);
                d.GetComponent<Image>().color = Color.white;
            }
            alienGo.Run(_activeAlienC, serversC, slots, false);
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
