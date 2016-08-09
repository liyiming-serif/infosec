using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class NetworkWindows : GUI, IHasTitle {

    AnimatorController victim;
    
    //TODO without frontend modification
    [SerializeField]
    Animator animator;
    [SerializeField]
    GameObject urlPanel;
    

    public string GetTitle()
    {
        return "Network";
    }

    protected void Awake()
    {
        base.Awake();
        this.Register(this.GetHashCode());
        victim = GetComponentInChildren<AnimatorController>();
    }

    public void SendVictimTo(List<Domain> url)
    {
        foreach(Domain dname in url)
        {
            Instantiate(dname, urlPanel.transform); // deep clone to network
        }
        string choice = ".clti"; //TODO decides layer by layer
        if (choice == ".clti")
        {
            victim.SetEndPosition(new Vector2(-447, -70));
        }
        else
        {
            victim.SetEndPosition(new Vector2(-126, -70));
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
            victim.ResetAnimator();
            animator.Rebind();
        }
    }
}
