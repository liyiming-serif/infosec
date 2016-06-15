using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class NewOutbox : MonoBehaviour{
	
	public Vector2 playerPos;

	[SerializeField] Transform slotsTransform;

	public void Initialise(Vector2 playerPos){
		this.playerPos = playerPos;
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
		
	public void EmptyBoxes()
	{
		foreach (Transform slotTransform in slotsTransform) {
			slotTransform.GetComponent<Slot> ().removeBox ();
		}
	}

	public void AcceptBox(NewBox box){
		if (box == null) {
			return;
		}
		if (GetCapacity () == 0) {
			slotsTransform.GetChild (GetMaxCapacity () - 1).GetComponent<Slot> ().removeBox ();
		}
		for (int i = GetCount () - 1; i >= 0; i--) {
			NewBox moveBox = slotsTransform.GetChild (i).GetComponent<Slot> ().item;
			moveBox.transform.SetParent (slotsTransform.GetChild (i+1));
		}
		box.transform.SetParent (slotsTransform.GetChild (0));
	}

}
