using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Outbox : MonoBehaviour{
	
	public Vector2 playerPos;

    [SerializeField]
	private Transform slotsTransform;

	public void Initialise(Vector2 playerPos){
		this.playerPos = playerPos;
	}

	public int GetMaxCapacity(){
		return slotsTransform.childCount;
	}

	public int GetCount(){
		int count = 0;
		foreach (Transform slotTransform in slotsTransform) {
			Data item = slotTransform.GetComponent<DataSlot> ().data;
			if (item != null) {
				count++;
			}
		}
		return count;
	}


	public int GetCapacity(){
		return GetMaxCapacity () - GetCount ();
	}
		
	public void EmptyAllData()
	{
		foreach (Transform slotTransform in slotsTransform) {
			slotTransform.GetComponent<DataSlot> ().RemoveData ();
		}
	}

	public void AcceptData(Data newData){
		if (newData == null) {
			return;
		}
		if (GetCapacity () == 0) {
			slotsTransform.GetChild (0).GetComponent<DataSlot> ().RemoveData ();
		}
		for (int i = GetCapacity(); i < GetMaxCapacity(); i++) {
			Data moveData = slotsTransform.GetChild (i).GetComponent<DataSlot> ().data;
			moveData.transform.SetParent (slotsTransform.GetChild (i - 1));
		}
		newData.transform.SetParent (slotsTransform.GetChild(GetMaxCapacity() - 1));
	}

	void Start (){
		slotsTransform = GetComponent<RectTransform> ();
	}
}
