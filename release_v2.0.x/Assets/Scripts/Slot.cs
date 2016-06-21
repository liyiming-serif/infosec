﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour {

	public TopCommand c {
		get{
			if (transform.childCount > 0) {
				return transform.GetChild (0).gameObject.GetComponent<TopCommand> ();
			}
			return null;
		}
	}

	public bool AcceptCommand (TopCommand newCommand){
		if (c) {
			return false;
		} else {
			newCommand.transform.SetParent (transform);
			return true;
		}
	}

	public void destructCommand(){
		if (c) {
			c.DestructSubCMD ();
			Destroy (c.gameObject);
		}
	}

	public void ActivateEventTrigger (bool toActive){
		c.GetComponent<CanvasGroup> ().blocksRaycasts = toActive;
	}
}
