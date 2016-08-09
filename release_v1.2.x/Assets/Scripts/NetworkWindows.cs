using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NetworkWindows : GUI, IHasTitle {

    AnimatorController victimController;
    List<Domain> urlString;
    
    [SerializeField]
    Animator victimAnimator;
    [SerializeField]
    GameObject urlPanel;
    [SerializeField]
    List<Domain> servers;
    
    public string GetTitle()
    {
        return "Network";
    }

    protected void Awake()
    {
        base.Awake();
        this.Register(this.GetHashCode());
        urlString = new List<Domain>();
    }

    private void Start()
    {
        victimController =  victimAnimator.GetComponent<AnimatorController>();
    }

    public void SendVictimTo(List<Domain> url)
    {
        foreach(Domain dname in url)
        {
            Object dclone = Instantiate(dname, urlPanel.transform); // deep clone to network
            urlString.Add((Domain) dclone);
        }
        //TODO fully working colouring.
        urlString[0].GetComponent<Image>().color = Color.green;
        servers[0].GetComponent<Image>().color = Color.green;
        string choice = ".clti"; //TODO decides layer by layer
        if (choice == ".clti")
        {
            victimController.SetEndPosition(new Vector2(-447, -70));
        }
        else
        {
            victimController.SetEndPosition(new Vector2(-126, -70));
        }
    }

    public void Result()
    {
        string choice = ".clti";
        if(choice == ".clti")
        {
            EditorUtility.DisplayDialog("Success!","The victim gave in her username and password.","Continue");
        }else
        {
            victimController.ResetAnimator();
            victimAnimator.Rebind();
        }
    }
}
