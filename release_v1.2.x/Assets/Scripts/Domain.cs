using UnityEngine;
using System.Collections;

public class Domain : MonoBehaviour {

    [SerializeField]
    private string domainName;

    public string dName
    {
        get
        {
            return domainName;
        }
        set
        {
            domainName = value;
        }
    }

}
