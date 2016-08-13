using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class NetworkWindows : GUI, IHasTitle, IEventSystemHandler {

    
    [SerializeField]
    List<Slot> urlString;
    [SerializeField]
    List<string> answer; //Answer needs to have a struct

    public static NetworkWindows instance;

    AlienC alienC;
    ServersGraphC serversC;
    int nowAt;
    
    public string GetTitle()
    {
        return "Network";
    }

    protected void Awake()
    {
        base.Awake();
        this.Register(this.GetHashCode());
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
            urlString[nowAt + 1].holding.GetComponent<Image>().color = Color.green;
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
        Domain tobeDestroyed = urlString[id].holding;
        if(tobeDestroyed)
        {
            Destroy(tobeDestroyed);
        }
        Instantiate(d, urlString[id].transform);
    }

}
