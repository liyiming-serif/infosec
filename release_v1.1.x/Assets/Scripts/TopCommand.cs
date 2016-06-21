using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public class TopCommand : MonoBehaviour, IUpdateSubCMDChoice{
	
	public enum Code {NoAction, Inbox, Outbox};

	public Code myCode;

	public SubCommand subCommandRef;

	#region IUpdateSubCMDChoice implementation

	void IUpdateSubCMDChoice.FinaliseSubCMDChoice (SubCommand.Code updateCode)
	{
		UpdateSubCommand (updateCode);
	}

	#endregion

	public void DestructSubCMD(){
		if (subCommandRef) {
			Destroy (subCommandRef.gameObject);
		}
	}

	public void UpdateSubCommand(SubCommand.Code newSubCode){
		DestructSubCMD ();
		switch (newSubCode) {
		case SubCommand.Code.Boss:
			subCommandRef = (Instantiate (Resources.Load ("BossCMDPrefab", typeof(SubCommand))) as SubCommand);
			break;
		case SubCommand.Code.Distrust:
			subCommandRef = (Instantiate (Resources.Load ("DistrustCMDPrefab", typeof(SubCommand))) as SubCommand);
			break;
		}
		subCommandRef.transform.SetParent (transform);
		subCommandRef.transform.localPosition = new Vector2 (80.0f, 0.0f);
	}
}

namespace UnityEngine.EventSystems {
	public interface IUpdateSubCMDChoice : IEventSystemHandler{
		void FinaliseSubCMDChoice (SubCommand.Code updateCode);
	}
}