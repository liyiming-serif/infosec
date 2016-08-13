using UnityEngine;
using UnityEngine.UI;

public class Domain : MonoBehaviour {

    public string dName
    {
        get
        {
            return GetComponentInChildren<Text>().text;
        }
        set
        {
            GetComponentInChildren<Text>().text = value;
        }
    }

}
