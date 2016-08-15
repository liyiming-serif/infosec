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

    AIGoScript AIGo; 
    Queue<AIController> AIs;
    AIController _activeAI;
    ServersGraphC serversC;
    List<Slot> slots;

    public AIController activeAI
    {
        get
        {
            return _activeAI;
        }
    }

    new void Awake()
    {
        base.Awake();
        Register(GetHashCode());
        slots = new List<Slot>();
        AIs = new Queue<AIController>();
        slots.AddRange(urlString.GetComponentsInChildren<Slot>());
        instance = this;
    }

    void Start()
    {
        serversC = GetComponentInChildren<ServersGraphC>();
        foreach(AIController AI in GetComponentsInChildren<AIController>())
        {
            AIs.Enqueue(AI);
        }
        AIGo = GetComponent<AIGoScript>();
        _activeAI = AIs.Dequeue();
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
        AIGo.Reset();
    }

    public void NextAI(AIController.CallbackFunct func)
    {
        Destroy(_activeAI.gameObject);
        _activeAI = null;
        if(AIs.Count > 0)
        {
            _activeAI = AIs.Dequeue();
            Reset();
            AIGo.Hint();
        }
        else
        {
            func();
        }
    }

    public void AlienGo(bool isForward)
    {
        if(!isForward)
        {
            //Reset ServersControlGraph since the AI returns to launchpad
            foreach (Slot s in slots)
            {
                Domain d = s.holding;
                serversC.LightupDomainName(d.dName, Color.white);
                d.GetComponent<Image>().color = Color.white;
            }
        }
        AIGo.Run(_activeAI, serversC, slots, isForward);
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
