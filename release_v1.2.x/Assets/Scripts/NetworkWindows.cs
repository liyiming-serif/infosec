using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NetworkWindows : GUI, IHasTitle {

    public AlienC alienC;
    public ServersGraphC serversC;

    [SerializeField]
    Animator alienA;
    [SerializeField]
    List<Slot> urlString;
    
    public string GetTitle()
    {
        return "Network";
    }

    protected void Awake()
    {
        base.Awake();
        this.Register(this.GetHashCode());
    }

    private void Start()
    {
        serversC = this.GetComponentInChildren<ServersGraphC>();
        alienC =  alienA.GetComponent<AlienC>();
    }

    public void AlienGo()
    {
        //TODO Tell serverC.
        //StartCoroutine(FindThePath(1));
    }

    public void updateNetworkURL(Domain d, int id)
    {
        Instantiate(d, urlString[id].transform);
    }

    //IEnumerator FindThePath(int id)
    //{
    //    yield return new WaitForSeconds(1);

    //    serversC.ToNextServer(1);
    //}

    //IEnumerator Done()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    alienA.gameObject.SetActive(false);
    //}

    //public void Explode()
    //{
    //    GameObject o = Instantiate(Resources.Load("Explosion"), alienA.transform) as GameObject;
    //    o.transform.localPosition = new Vector2(-50, 35);
    //    StartCoroutine(Done());
    //}
    //public void Result()
    //{
    //    serversC.ArriveNextServer();
    //}
}
