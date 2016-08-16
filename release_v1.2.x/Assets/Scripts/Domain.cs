using UnityEngine;
using UnityEngine.UI;

public class Domain : MonoBehaviour {

    public string myName
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

    public Image myImage
    {
        get
        {
            return GetComponent<Image>();
        }
    }
}
