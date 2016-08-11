using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NetworkW : GUI, IHasTitle {

    public AlienController alienC;
    public ServersGraphController serversC;
    public List<Domain> urlString;

    [SerializeField]
    Animator alienA;
    [SerializeField]
    GameObject urlSPanel;
    [SerializeField]
    List<Domain> dnames;
    
    public string GetTitle()
    {
        return "Network";
    }

    protected void Awake()
    {
        base.Awake();
        this.Register(this.GetHashCode());
        urlString = new List<Domain>();
        serversC = this.GetComponentInChildren<ServersGraphController>();
    }

    private void Start()
    {
        alienC =  alienA.GetComponent<AlienController>();
    }

    public void SendVictimTo(List<Domain> url)
    {
        if(!url[0])
        {
            ReturnTaskManager().apps.animator.Rebind();
            return;
        }

        foreach(Domain dname in urlSPanel.GetComponentsInChildren<Domain>())
        {
            //Object dclone = Instantiate(dname, urlSPanel.transform); // deep clone to network
            urlString.Add((Domain) dname);
        }

        urlString[0].GetComponent<Image>().color = Color.green;
        StartCoroutine(FindThePath(1));
    }

    IEnumerator FindThePath(int id)
    {
        yield return new WaitForSeconds(1);
        dnames[0].GetComponent<Image>().color = Color.green;
        serversC.ToNextServer(1);
    }

    IEnumerator Done()
    {
        yield return new WaitForSeconds(0.5f);
        alienA.gameObject.SetActive(false);
    }

    public void Explode()
    {
        GameObject o = Instantiate(Resources.Load("Explosion"), alienA.transform) as GameObject;
        o.transform.localPosition = new Vector2(-50, 35);
        StartCoroutine(Done());
    }
    public void Result()
    {
        serversC.ArriveNextServer();
    }
}
