using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ServersGraphC : MonoBehaviour
{

    public Server root;

    Server _goingTo;

    public Server goingTo
    {
        get
        {
            return _goingTo;
        }
        set
        {
            _goingTo = value;
        }
    }

    Server _nowAt;

    public Server nowAt
    {
        get
        {
            return _nowAt;
        }
        set
        {
            _nowAt = value;
        }
    }

    public Color GetLightedColour()
    {
        Color lighted = Color.green;
        lighted.a = 0.5f;
        return lighted;
    }

    public Color GetDefaultColour()
    {
        Color original = Color.white;
        original.a = 0.5f;
        return original;
    }

    public void SetNewLocation(Server newServer)
    {
        goingTo = newServer;
    }

    public void ResetAllDomains()
    {
        root.SetAllDomainsColour(GetDefaultColour());
    }

    
    public void Reset()
    {
        goingTo = root;
        nowAt = null;
        ResetAllDomains();
    }

    void Start()
    {
        Reset();
    }

}
