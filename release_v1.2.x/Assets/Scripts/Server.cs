using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class Server : MonoBehaviour {

    Server parent;
    List<Server> children;
    Domain dLabel;
    Animator inpath;

    public Server ReturnParent()
    {
        return parent;
    }
    public void SetDomainColour(Color c)
    {
        dLabel.myImage.color = c;
    }

    public void BucklePath(bool isEnabled)
    {
        if (isEnabled)
        {
            inpath.SetTrigger("Buckle");
        }
        else
        {
            inpath.SetTrigger("Unbuckle");
        }
    }

    public Vector2 GetLandingPos()
    {
        Vector2 result = transform.position;
        result.x += 30;
        result.y -= 30;
        return result;
    }

    public Server ReturnChild(string name)
    {
        Server result = null;
        foreach(Server s in children)
        {
            if(s.dLabel.myName == name)
            {
                result = s;
                break;
            }
        }
        return result;
    }

    // Use this for initialization
    void Awake () {
        parent = transform.parent.GetComponent<Server>();
        children = new List<Server>();
        foreach(Transform trans in transform)
        {
            Server s = trans.GetComponent<Server>();
            if (s)
            {
                children.Add(s);
            }
            Domain d = trans.GetComponent<Domain>();
            if(d)
            {
                dLabel = d;
            }
            if(trans.tag == "Platform")
            {
                inpath = trans.GetComponent<Animator>();
            }
        }
        Assert.IsNotNull(dLabel);
        Assert.IsNotNull(inpath);
	}
	
    public void SetAllDomainsColour(Color c)
    {
        foreach(Server s in children)
        {
            s.SetAllDomainsColour(c);
        }
        SetDomainColour(c);
    }

	public void BuckleAllPaths(bool isEnabled)
    {
        foreach(Server s in children)
        {
            s.BuckleAllPaths(isEnabled);
        }
        BucklePath(isEnabled);
    }

}
