using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialOne : MonoBehaviour {

	void Start () {
        if (NetworkWindows.instance.gameObject.activeSelf)
        {
            NetworkWindows.instance.SetSelfVisible(false);
        }
        TaskManager.instance.LookUpAppSpawn("Ping!").Dance();
	}
}
