using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NetworkWindows : GUI, IHasTitle {

    AlienController alienC;
    ServersGraphController serversC;
    List<Domain> urlString;

    [SerializeField]
    Animator victimAnimator;
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
        alienC =  victimAnimator.GetComponent<AlienController>();
    }

    public void SendVictimTo(List<Domain> url)
    {
        foreach(Domain dname in url)
        {
            Object dclone = Instantiate(dname, urlSPanel.transform); // deep clone to network
            urlString.Add((Domain) dclone);
        }
        //TODO fully working colouring.
        serversC.ToNextServer(1);
        urlString[0].GetComponent<Image>().color = Color.green;
        dnames[0].GetComponent<Image>().color = Color.green;
        string choice = ".clti"; //TODO decides layer by layer
        if (choice == ".clti")
        {
            alienC.SetEndPosition(new Vector2(-292, 140));
        }
        else
        {
            alienC.SetEndPosition(new Vector2(-126, -70));
        }

    }

    public void Result()
    {
        string choice = ".clti";
        if(choice == ".clti")
        {
            serversC.ArriveNextServer();
            //EditorUtility.DisplayDialog("Success!","The victim gave in her username and password.","Continue");
        }else
        {
            alienC.ResetAnimator();
            victimAnimator.Rebind();
        }
    }
}
