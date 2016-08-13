using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class NetworkWindows : GUI, IHasTitle, IEventSystemHandler {

    
    [SerializeField]
    GameObject urlString;

    public static NetworkWindows instance;

    AlienC alienC;
    ServersGraphC serversC;

    List<Slot> slots;
    int nowAt;
    
    public string GetTitle()
    {
        return "Network";
    }

    protected void Awake()
    {
        base.Awake();
        this.Register(this.GetHashCode());
        slots = new List<Slot>();
        slots.AddRange(urlString.GetComponentsInChildren<Slot>());
        nowAt = -1;
        instance = this;
    }

    private void Start()
    {
        serversC = this.GetComponentInChildren<ServersGraphC>();
        alienC = this.GetComponentInChildren<AlienC>();
    }

    public void AlienGo()
    {
        if(nowAt == -1)
        {
            slots[nowAt + 1].holding.GetComponent<Image>().color = Color.green;
            serversC.LightupDomainName(nowAt + 1);
            serversC.ActivatePath(nowAt + 1, true);
            alienC.SetEndPosition(serversC.GetLandingPos(nowAt + 1));
            nowAt += 1;
        }
        else if(nowAt == 0)
        {
            serversC.ActivatePath(nowAt, false);
            alienC.GetExploded();
        }
    }

    public void updateNetworkURL(Domain d, int id)
    {
        Domain tobeDestroyed = slots[id].holding;
        if(tobeDestroyed)
        {
            Destroy(tobeDestroyed);
        }
        Instantiate(d, slots[id].transform);
    }

}
