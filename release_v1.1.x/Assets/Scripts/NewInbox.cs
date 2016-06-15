using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NewInbox : MonoBehaviour{
	
	public Vector2 playerPos;
	[SerializeField] Transform slotsTransform;

	public void Initialise(Vector2 playerPos, NewBox[] initBoxes = null){
		this.playerPos = playerPos;
		if (initBoxes != null) {
			for (int i = 0; i < initBoxes.Length; i++) {
				initBoxes[i].transform.SetParent(slotsTransform.GetChild(initBoxes.Length - 1 - i));
			}
		}
	}

	public int GetMaxCapacity(){
		return slotsTransform.childCount;
	}

	public int GetCount(){
		int count = 0;
		foreach (Transform slotTransform in slotsTransform) {
			NewBox item = slotTransform.GetComponent<Slot> ().item;
			if (item != null) {
				count++;
			}
		}
		return count;
	}


	public int GetCapacity(){
		return GetMaxCapacity () - GetCount ();
	}


	public void ResetInbox(NewBox[] initBoxes = null) {
		EmptyBoxes ();
		if (initBoxes != null) {
			for (int i = 0; i < initBoxes.Length; i++) {
				initBoxes[i].transform.SetParent(slotsTransform.GetChild(initBoxes.Length - 1 - i));
			}
		}
	}

	public void EmptyBoxes()
	{
		foreach (Transform slotTransform in slotsTransform) {
			slotTransform.GetComponent<Slot> ().removeBox ();
		}
	}
		
	public NewBox sendBox(int index){
		if (GetCount () <= index) {
			return null;
		}
		NewBox item = slotsTransform.GetChild (GetMaxCapacity () - 1 - index).GetComponent<Slot> ().item;
		for (int i = index+1; i < GetCount (); i++) {
			NewBox moveBox = slotsTransform.GetChild (GetMaxCapacity() - 1 - i).GetComponent<Slot>().item;
			moveBox.transform.SetParent (slotsTransform.GetChild (GetMaxCapacity() - i));
		}
		return item;
	}


	public NewBox sendFirstBox(){
		return sendBox (0);
	}

}
