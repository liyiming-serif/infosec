using UnityEngine;
using System.Collections;

public class Network : GUI, IHasTitle {

    public string GetTitle()
    {
        return "Network";
    }

    protected void Awake()
    {
        base.Awake();
        this.Register(this.GetHashCode());
    }
}
