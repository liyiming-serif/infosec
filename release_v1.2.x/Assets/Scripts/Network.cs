using UnityEngine;
using System.Collections;
using UnityEditor;

public class Network : GUI, IHasTitle {

    AnimatorController victim;
    string choice;

    //TODO decouple
    [SerializeField]
    Animator animator;

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

    public void SendVictimTo(string choice)
    {
        this.choice = choice;
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
