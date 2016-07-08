using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class TopCommand : MonoBehaviour, IUpdateSubCMDChoice{
	
	public enum Code {NoAction, Inbox, Outbox, Load, Store, BossOwns};

	public Code myCode;

	public SubCommand subCommandRef;

	#region IUpdateSubCMDChoice implementation

	void IUpdateSubCMDChoice.FinaliseSubCMDChoice (SubCommand.Code updateCode)
	{
        foreach (TopCommandSlot s in GameObject.FindObjectsOfType<TopCommandSlot>())
        {
            s.ActivateEventTrigger(true);
        }
        UpdateSubCommand(updateCode);
	}

	#endregion

	public void DestructSubCMD(){
		if (subCommandRef) {
			Destroy (subCommandRef.gameObject);
		}
	}

	/**End Arg Update*/
	public void UpdateSubCommand(SubCommand.Code newSubCode) { 
		DestructSubCMD ();
		switch (newSubCode) {
		case SubCommand.Code.Boss:
			subCommandRef = (Instantiate (Resources.Load ("BossCMDPrefab", typeof(SubCommand))) as SubCommand);
			break;
		case SubCommand.Code.Distrust:
			subCommandRef = (Instantiate (Resources.Load ("DistrustCMDPrefab", typeof(SubCommand))) as SubCommand);
			break;
		case SubCommand.Code.Zero:
			subCommandRef = (Instantiate (Resources.Load ("ZeroCMDPrefab", typeof(SubCommand))) as SubCommand);
			break;
		case SubCommand.Code.One:
			subCommandRef = (Instantiate (Resources.Load ("OneCMDPrefab", typeof(SubCommand))) as SubCommand);
			break;
		}
		subCommandRef.transform.SetParent (transform);
		subCommandRef.transform.localPosition = new Vector2 (80.0f, 0.0f);
		foreach(Transform child in subCommandRef.transform)
        	child.gameObject.SetActive(false);
    }

 
}

namespace UnityEngine.EventSystems {
	public interface IUpdateSubCMDChoice : IEventSystemHandler{
		void FinaliseSubCMDChoice (SubCommand.Code updateCode);
	}
}