using UnityEngine;
using System.Collections;

public class TutorialTwo : MonoBehaviour {

    Slot s;
	// Use this for initialization
	void Start () {
        s = GetComponentInChildren<Slot>();
	}
	
	// Update is called once per frame
	void Update () {
        if (s.holding)
        {
            NetworkWindows.instance.SetSelfVisible(true);
        }
	}
}
